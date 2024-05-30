using System.Text.Json;
using System.Text.Json.Serialization;

namespace DaNangTourism.Server.ModelBindingConverter
{
    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        private const string TimeFormat = "HH:mm"; // Định dạng thời gian

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                if (reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException($"Unexpected token type: {reader.TokenType}. Expected a string.");
                }

                var timeString = reader.GetString();
                if (string.IsNullOrEmpty(timeString))
                {
                    throw new JsonException("The time string is null or empty.");
                }

                return TimeOnly.ParseExact(timeString, TimeFormat);
            }
            catch (FormatException)
            {
                throw new JsonException($"The time string '{reader.GetString()}' was not in the expected format '{TimeFormat}'.");
            }
            catch (Exception ex)
            {
                throw new JsonException("An error occurred while reading the TimeOnly value.", ex);
            }
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            try
            {
                writer.WriteStringValue(value.ToString(TimeFormat));
            }
            catch (Exception ex)
            {
                throw new JsonException("An error occurred while writing the TimeOnly value.", ex);
            }
        }
    }
}
