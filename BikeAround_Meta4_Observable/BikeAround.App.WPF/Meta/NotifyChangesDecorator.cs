using CSharp.Meta;
using System.Reflection;

namespace BikeAround.App.Meta
{
    internal class NotifyChangesDecorator : Decorator
    {
        public override void DecoratePropertySet(PropertyInfo property, object thisObject, object value)
        {
            if (typeof(INotifyPropertyChangesEx).IsAssignableFrom(MetaPrimitives.ThisObjectType(property)))
            {
                object oldValue = property.GetValue(thisObject);
                if (Equals(oldValue, value))
                    return;

                var notifier = (INotifyPropertyChangesEx)thisObject;
                notifier.RaisePropertyChanging(property.Name);
                property.SetValue(thisObject, value);
                notifier.RaisePropertyChanged(property.Name);
            }
            else
            {
                throw new MetaException("NotifyChangesDecorator can only be applied to properties of types which implement INotifyPropertyChangesEx.");
            }
        }

        public override object DecoratePropertyGet(PropertyInfo property, object thisObject)
        {
            return property.GetValue(thisObject);
        }
    }
}