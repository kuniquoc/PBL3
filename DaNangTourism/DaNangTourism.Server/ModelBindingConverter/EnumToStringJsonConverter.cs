using System.Text.Json.Serialization;
using System.Text.Json;

namespace DaNangTourism.Server.ModelBindingConverter
{
    public class EnumToStringJsonConverter<T> : JsonConverter<T> where T : Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                if (reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException($"Unexpected token type: {reader.TokenType}. Expected a string.");
                }
                var enumString = reader.GetString();
                if (string.IsNullOrEmpty(enumString))
                {
                    throw new JsonException("The enum string is null or empty.");
                }
                return (T)Enum.Parse(typeof(T), enumString, true);
            }
            catch (Exception ex)
            {
                throw new JsonException("An error occurred while reading the Enum value.", ex);
            }
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
