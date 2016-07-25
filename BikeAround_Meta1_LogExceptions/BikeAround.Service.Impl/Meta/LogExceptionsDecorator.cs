using CSharp.Meta;
using System;
using System.Reflection;

namespace BikeAround.Service.Impl.Meta
{
    internal sealed class LogExceptionsDecorator : Decorator
    {
        public ILogExceptions ExceptionLog { get; }

        public LogExceptionsDecorator(ILogExceptions exceptionLog)
        {
            ExceptionLog = exceptionLog;
        }

        public override object DecorateMethod(MethodInfo method, object thisObject, object[] arguments)
        {
            try
            {
                return method.Invoke(thisObject, arguments);
            }
            catch (Exception ex)
            {
                ExceptionLog.LogException(ex);
                throw;
            }
        }
    }
}