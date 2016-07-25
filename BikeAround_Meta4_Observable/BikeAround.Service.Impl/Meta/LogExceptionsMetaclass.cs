using CSharp.Meta;
using System;
using System.Reflection;

namespace BikeAround.Service.Impl.Meta
{
    internal sealed class LogExceptionsMetaclass : Metaclass
    {
        public ILogExceptions ExceptionLog { get; }

        public bool OnlyPublicMethods { get; }

        public LogExceptionsMetaclass(ILogExceptions exceptionLog, bool onlyPublicMethods = true)
        {
            ExceptionLog = exceptionLog;
            OnlyPublicMethods = onlyPublicMethods;
        }

        public override void ApplyToType(Type type)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
            if (!OnlyPublicMethods)
            {
                bindingFlags |= BindingFlags.NonPublic;
            }
            MethodInfo[] methods = type.GetMethods(bindingFlags);
            foreach (MethodInfo method in methods)
            {
                MetaPrimitives.ApplyDecorator(method, new LogExceptionsDecorator(ExceptionLog));
            }
        }
    }
}