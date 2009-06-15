﻿namespace QI4N.Framework.Runtime
{
    using System.Diagnostics;
    using System.Reflection;

    public class CompositeMethodModel
    {
        private readonly MethodInfo method;

        private readonly int mixinIndex;

        private readonly MixinModel mixinModel;

        public CompositeMethodModel(MethodInfo method, MixinModel model, int mixinIndex)
        {
            this.method = method;
            this.mixinModel = model;
            this.mixinIndex = mixinIndex;
        }

#if !DEBUG
        [DebuggerStepThrough]
        [DebuggerHidden]
#endif
        public object Invoke(object proxy, object[] args, MixinsInstance mixins, ModuleInstance moduleInstance)
        {
            CompositeMethodInstance methodInstance = this.GetInstance(moduleInstance);

            return mixins.Invoke(proxy, args, methodInstance);
        }

#if !DEBUG
        [DebuggerStepThrough]
        [DebuggerHidden]
#endif
        private CompositeMethodInstance GetInstance(ModuleInstance moduleInstance)
        {
            return this.newCompositeMethodInstance(moduleInstance);
        }

#if !DEBUG
        [DebuggerStepThrough]
        [DebuggerHidden]
#endif
        private CompositeMethodInstance newCompositeMethodInstance(ModuleInstance moduleInstance)
        {
            FragmentInvocationHandler mixinInvocationHandler = this.mixinModel.NewInvocationHandler(this.method);
            InvocationHandler invoker = mixinInvocationHandler;
            //if (methodConcerns.hasConcerns())
            //{
            //    MethodConcernsInstance concernsInstance = methodConcerns.newInstance(moduleInstance, mixinInvocationHandler);
            //    invoker = concernsInstance;
            //}
            //if (methodSideEffects.hasSideEffects())
            //{
            //    MethodSideEffectsInstance sideEffectsInstance = methodSideEffects.newInstance(moduleInstance, invoker);
            //    invoker = sideEffectsInstance;
            //}

            return new CompositeMethodInstance(invoker, mixinInvocationHandler, this.method, this.mixinIndex);
        }
    }

    public abstract class FragmentInvocationHandler : InvocationHandler
    {
        protected object fragment;

        public abstract object Invoke(object proxy, MethodInfo method, object[] args);

#if !DEBUG
        [DebuggerStepThrough]
        [DebuggerHidden]
#endif
        public void SetFragment(object fragment)
        {
            this.fragment = fragment;
        }
    }
}