﻿namespace QI4N.Framework.Reflection
{
    using System;
    using System.Reflection;

    using Activation;

    public static class Proxy
    {
        public static InvocationHandler GetInvocationHandler(object proxy)
        {
            FieldInfo defaultHandlerField = proxy.GetType().GetField("defaultHandler");
            var handler = defaultHandlerField.GetValue(proxy) as InvocationHandler;
            return handler;
        }

        public static bool IsProxyClass(Type type)
        {
            FieldInfo defaultHandlerField = type.GetField("defaultHandler");
            return defaultHandlerField != null;
        }

        public static object NewProxyInstance(Type type, InvocationHandler handler)
        {
            var proxyBuilder = new InvocationProxyTypeBuilder();

            Type proxyType = proxyBuilder.BuildProxyType(type);
            var instance = Activator.CreateInstance(proxyType, new object[]
                                                                   {
                                                                           handler
                                                                   });

            return instance;
        }
    }
}