using System;
using System.Linq.Expressions;

namespace Framework.QueryEngine
{
    public static class ObjectExtensions
    {
        public static T GetPropertyValue<T>(this object obj, string property)
        {
            return (T)obj.GetType().GetProperty(property).GetValue(obj, null);
        }

        public static T GetPropertyValue<T>(this object obj, Expression<Func<object, object>> property)
        {
            var propertyName = ExpressionHelper.GetMemberName(property);
            return (T)obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
    }
}