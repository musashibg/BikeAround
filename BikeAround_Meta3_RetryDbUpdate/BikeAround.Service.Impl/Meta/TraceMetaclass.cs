using CSharp.Meta;
using System;
using System.Reflection;

namespace BikeAround.Service.Impl.Meta
{
    internal sealed class TraceMetaclass : Metaclass
    {
        public ILogTraceEntries TraceLog { get; }

        public bool OnlyPublicMethods { get; }

        public TraceMetaclass(ILogTraceEntries traceLog, bool onlyPublicMethods = true)
        {
            TraceLog = traceLog;
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
                MetaPrimitives.ApplyDecorator(method, new TraceDecorator(TraceLog));
            }
        }
    }
}