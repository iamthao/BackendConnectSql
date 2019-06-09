using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace Framework.Utility
{
    public class DefaultWrongFormatDeserialize : DateTimeConverterBase
    {
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.Value == null) return null;
            if (string.IsNullOrEmpty(reader.Value.ToString())) return null;
            DateTime result;

            result = DateTime.TryParse(reader.Value.ToString(), out result) ? result : DateTime.MinValue;

            if (reader.Value.ToString().Length > 10)
                return result.ToUniversalTime();

            result = DateTime.SpecifyKind(result, DateTimeKind.Utc);

            return result;
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            WriteJson(writer, value, serializer);
        }
    }
}
