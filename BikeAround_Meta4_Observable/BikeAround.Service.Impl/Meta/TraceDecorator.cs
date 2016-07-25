using CSharp.Meta;
using System.Reflection;

namespace BikeAround.Service.Impl.Meta
{
    internal sealed class TraceDecorator : Decorator
    {
        public ILogTraceEntries TraceLog { get; }

        public TraceDecorator(ILogTraceEntries traceLog)
        {
            TraceLog = traceLog;
        }

        public override object DecorateMethod(MethodInfo method, object thisObject, object[] arguments)
        {
            ParameterInfo[] parameters = method.GetParameters();
            var argumentStrings = new string[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo parameter = parameters[i];
                if (parameter.GetCustomAttribute<DoNotTraceAttribute>() == null)
                {
                    argumentStrings[i] = arguments[i].ToString();
                }
                else
                {
                    argumentStrings[i] = "<" + parameter.Name + ">";
                }
            }

            string methodDescription;
            if (parameters.Length == 0)
            {
                methodDescription = $"{method.DeclaringType}.{method.Name}()";
            }
            else
            {
                methodDescription = $"{method.DeclaringType}.{method.Name}({string.Join(", ", argumentStrings)})";
            }
            TraceLog.LogTraceEntry($"Entering {methodDescription}");

            object result = method.Invoke(thisObject, arguments);

            if (method.ReturnType == typeof(void))
            {
                TraceLog.LogTraceEntry($"Exiting {methodDescription}");
            }
            else
            {
                TraceLog.LogTraceEntry($"Exiting {methodDescription} with result {result}");
            }

            return result;
        }
    }
}