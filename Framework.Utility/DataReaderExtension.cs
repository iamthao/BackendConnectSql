using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Framework.Utility
{

    public static class DataReaderExtension
    {
        public static List<T> MapToList<T>(this DbDataReader dr) where T : new()
        {
            if (dr == null || !dr.HasRows) return null;
            var entity = typeof(T);
            var entities = new List<T>();
            var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var propDict = props.ToDictionary(p => p.Name.ToUpper(), p => p);

            while (dr.Read())
            {
                var newObject = new T();
                for (var index = 0; index < dr.FieldCount; index++)
                {
                    if (!propDict.ContainsKey(dr.GetName(index).ToUpper())) continue;
                    var info = propDict[dr.GetName(index).ToUpper()];
                    if ((info == null) || !info.CanWrite) continue;
                    var val = dr.GetValue(index);
                    SetValueWithDataType(info, newObject, val);
                }
                entities.Add(newObject);
            }
            return entities;
        }

        public static T MapToObject<T>(this DbDataReader dr)
            where T : new()
        {
            if (dr == null || !dr.HasRows) return default(T);
            var entity = typeof(T);
            var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var propDict = props.ToDictionary(p => p.Name.ToUpper(), p => p);

            while (dr.Read())
            {
                var newObject = new T();
                for (var index = 0; index < dr.FieldCount; index++)
                {
                    if (!propDict.ContainsKey(dr.GetName(index).ToUpper())) continue;
                    var info = propDict[dr.GetName(index).ToUpper()];
                    if ((info == null) || !info.CanWrite) continue;
                    var val = dr.GetValue(index);
                    SetValueWithDataType(info, newObject, val);
                }
                return newObject;
            }
            return default(T);
        }

        private static void SetValueWithDataType<T>(PropertyInfo info, T newObject, object val) where T : new()
        {
            switch (info.PropertyType.Name)
            {
                case "Boolean":
                    info.SetValue(newObject, (val != DBNull.Value && val.ToString().Equals("1")), null);
                    break;
                default:
                    info.SetValue(newObject, (val == DBNull.Value) ? null : val, null);
                    break;
            }
        }


        public static ExpandoObject MapToExpandoObject(this DbDataReader dr)
        {
            var result = new ExpandoObject();
            var expandoDic = (IDictionary<string, object>)result;

            while (dr.Read())
            {
                for (var i = 0; i < dr.FieldCount; i++)
                {
                    expandoDic.Add(dr.GetName(i), dr.GetValue(i));
                }
                return result;
            }
            return result;
        }

        public static List<ExpandoObject> MapToListExpandoObject(this DbDataReader dr)
        {
            var result = new List<ExpandoObject>();
            while (dr.Read())
            {
                var expando = new ExpandoObject();
                var expandoDic = (IDictionary<string, object>)expando;
                for (var i = 0; i < dr.FieldCount; i++)
                {
                    expandoDic.Add(dr.GetName(i), dr.GetValue(i));
                }
                result.Add(expando);
            }
            return result;
        }


    }
}
