using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace DaNangTourism.Server.ModelBindingConverter
{
    public class ConvertToISO8061DateTime : JsonConverter<DateTime>
    {
        private const string Iso8601Format = "yyyy-MM-ddTHH:mm:ss.fffZ";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Unexpected token parsing date. Expected String, got {reader.TokenType}.");
            }

            if (!DateTime.TryParseExact(reader.GetString(), Iso8601Format, null, System.Globalization.DateTimeStyles.None, out DateTime dateTime))
            {
                throw new JsonException($"Invalid date format: '{reader.GetString()}'.");
            }

            return dateTime;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime().ToString(Iso8601Format));
        }
    }
}
