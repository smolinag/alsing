﻿namespace QI4N.Framework.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    using Reflection;

    public class CompositeMethodsModel
    {
        private readonly Type compositeType;

        private readonly IDictionary<MethodInfo, CompositeMethodModel> methods;

        private readonly MixinsModel mixinsModel;

        public IEnumerable<MethodInfo> Methods
        {
            get
            {
                foreach(var method in methods.Keys)
                    yield return method;
            }
        }

        public CompositeMethodsModel(Type compositeType, MixinsModel mixinsModel)
        {
            this.methods = new Dictionary<MethodInfo, CompositeMethodModel>();
            this.compositeType = compositeType;
            this.mixinsModel = mixinsModel;
           // this.implementMixinType(compositeType);
            this.BuildMixinsModel(compositeType);
            this.ImplementMixinMethods();
        }

        [DebuggerStepThrough]
        [DebuggerHidden]
        public object Invoke(MixinsInstance mixins, object proxy, MethodInfo method, object[] args, ModuleInstance moduleInstance)
        {
            CompositeMethodModel compositeMethod;

            if (this.methods.TryGetValue(method, out compositeMethod) == false)
            {
                return mixins.InvokeObject(proxy, args, method);
            }

            return compositeMethod.Invoke(proxy, args, mixins, moduleInstance);
        }

        //public void implementMixinType(Type mixinType)
        //{
        //    var thisDependencies = new HashSet<Type>();
        //    foreach (MethodInfo method in mixinType.GetAllInterfaceMethods())
        //    {
        //        if (methods.ContainsKey(method) == false)
        //        {
        //            //    MethodConcernsModel methodConcernsModel = concernsModel.concernsFor( method, type );
        //            //    MethodSideEffectsModel methodSideEffectsModel1 = sideEffectsModel.sideEffectsFor( method, type );

        //            MixinModel mixinModel = mixinsModel.ImplementMethod(method);
        //            //    MethodConcernsModel mixinMethodConcernsModel = mixinModel.concernsFor( method, type );
        //            //    methodConcernsModel = methodConcernsModel.combineWith( mixinMethodConcernsModel );
        //            //    MethodSideEffectsModel mixinMethodSideEffectsModel = mixinModel.sideEffectsFor( method, type );
        //            //    methodSideEffectsModel1 = methodSideEffectsModel1.combineWith( mixinMethodSideEffectsModel );
        //            //    method.setAccessible( true );
        //            var compositeMethodModel = new CompositeMethodModel(method, mixinModel, this.mixinsModel.IndexOfMixin(mixinModel.MixinType));

        //            // Implement @This references
        //            //    methodComposite.addThisInjections( thisDependencies );
        //            //    mixinModel.addThisInjections( thisDependencies );
        //            //    methods.put( method, methodComposite );
        //            this.methods.Add(method, compositeMethodModel);
        //        }
        //    }

        //    // Add type to set of mixin types
        //    mixinsModel.AddMixinType(mixinType);

        //    //// Implement all @This dependencies that were found
        //    foreach(Type thisDependency in thisDependencies )
        //    {
        //        implementMixinType( thisDependency );
        //    }
        //}

        private void BuildMixinsModel(Type mixinType)
        {
            IEnumerable<Type> allInterfaces = mixinType.GetAllInterfaces();

            //add interfaces for Composite
            foreach (Type mixin in allInterfaces)
            {
                this.mixinsModel.AddMixinType(mixin);
            }

            mixinsModel.AddThisTypes();

        }

        private void ImplementMixinMethods()
        {
            var mixinTypes = mixinsModel.GetMixinTypes();
            foreach (var mixinType in mixinTypes)
            {
                foreach (MethodInfo method in mixinType.GetMethods())
                {
                    MixinModel mixinModel = this.mixinsModel.ImplementMethod(method);
                    var compositeMethodModel = new CompositeMethodModel(method, mixinModel, this.mixinsModel.IndexOfMixin(mixinModel.MixinType));
                    this.methods.Add(method, compositeMethodModel);
                }
            }
        }
    }
}