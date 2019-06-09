using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;

namespace Framework.Utility
{
    public static class DynamicExtensions
    {
        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return expando as dynamic;
        }

        public static Dictionary<string, object> ToDictionary(this object value)
        {
            return TypeDescriptor.GetProperties(value.GetType()).Cast<PropertyDescriptor>().ToDictionary(property => property.Name, property => property.GetValue(value));
        }
        public static bool IsPropertyExist(dynamic settings, string name)
        {
            try
            {
                var value = settings[name].Value;
                return true;
            }
            catch (RuntimeBinderException)
            {

                return false;
            }
        }
    }
}