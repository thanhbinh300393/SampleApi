using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sample.Common.Extentions
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // yyyy-MM-dd hh:mm:ss
            string str = reader.GetString();
            int year = str.Substring(0, 4).ToInt();
            int month = str.Substring(5, 2).ToInt();
            int day = str.Substring(8, 2).ToInt();
            if (str.Length < 11)
                return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Local);
            int hour = str.Substring(11, 2).ToInt();
            int minute = str.Substring(14, 2).ToInt();
            int second = str.Substring(17, 2).ToInt();
            return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Local);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss +00"));
        }
    }
}
