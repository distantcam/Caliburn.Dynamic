using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Caliburn.Dynamic
{
    partial class BindableObject : DynamicObject
    {
        private Dictionary<string, object> dynamicProperties = new Dictionary<string, object>();
        private Dictionary<string, PropertyInfo> staticProperties = null;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (dynamicProperties.TryGetValue(binder.Name, out result))
            {
                return true;
            }

            PreloadStaticProperties();

            PropertyInfo property;
            if (staticProperties.TryGetValue(binder.Name, out property) && property.CanRead)
            {
                result = property.GetValue(this);
                return true;
            }

            result = null;
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            PreloadStaticProperties();

            PropertyInfo property;
            if (staticProperties.TryGetValue(binder.Name, out property))
            {
                if (!property.CanWrite)
                {
                    return false;
                }
                property.SetValue(this, value);
                return true;
            }

            object result;
            if (!dynamicProperties.TryGetValue(binder.Name, out result))
            {
                result = null;
            }

            if (!Equals(result, value))
            {
                OnPropertyChanging(binder.Name, result);
                dynamicProperties[binder.Name] = value;
                OnPropertyChanged(binder.Name, result, value);
            }
            return true;
        }

        private void PreloadStaticProperties()
        {
            if (staticProperties != null)
            {
                return;
            }

            var type = this.GetType();

            staticProperties = type.GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .OfType<PropertyInfo>()
                .ToDictionary(m => m.Name);
        }
    }
}