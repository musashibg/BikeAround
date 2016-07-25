using CSharp.Meta;
using System;
using System.Reflection;

namespace BikeAround.App.Meta
{
    internal class ObservableMetaclass : Metaclass
    {
        public override void ApplyToType(Type type)
        {
            // The type's parent type might already include the NotifyPropertyChangesTrait
            if (!typeof(INotifyPropertyChangesEx).IsAssignableFrom(type))
                MetaPrimitives.AddTrait<NotifyPropertyChangesTrait>(type);

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo property in properties)
            {
                if (!MetaPrimitives.IsReadOnly(property))
                    MetaPrimitives.ApplyDecorator(property, new NotifyChangesDecorator());
            }
        }
    }
}