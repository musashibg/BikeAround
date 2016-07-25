using System;

namespace BikeAround.Service.Impl.Meta
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class DoNotTraceAttribute : Attribute
    {
    }
}