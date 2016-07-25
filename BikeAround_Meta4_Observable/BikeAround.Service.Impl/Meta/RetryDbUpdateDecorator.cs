using CSharp.Meta;
using System.Data.Entity.Infrastructure;
using System.Reflection;

namespace BikeAround.Service.Impl.Meta
{
    internal sealed class RetryDbUpdateDecorator : Decorator
    {
        public override object DecorateMethod(MethodInfo method, object thisObject, object[] arguments)
        {
            if (typeof(IRevertDbChanges).IsAssignableFrom(MetaPrimitives.ThisObjectType(method)))
            {
                object result = MetaPrimitives.DefaultValue(method.ReturnType);
                bool savedSuccessfully;
                do
                {
                    // Repeatedly execute the business logic until the changes can be safely saved to the database
                    try
                    {
                        result = method.Invoke(thisObject, arguments);
                        savedSuccessfully = true;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        ((IRevertDbChanges)thisObject).RevertChanges();
                        savedSuccessfully = false;
                    }
                }
                while (!savedSuccessfully);

                return result;
            }
            else
            {
                throw new MetaException("RetryDbUpdatesDecorator can only be applied to members of a type which implements the interface IRevertDbChanges.");
            }
        }
    }
}