using System;
using Newtonsoft.Json;
using Starter.DateTimeTypes;

namespace Starter.DateTimeTypes.NewtonsoftJson {
    public class UtcDateTimeJsonConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return objectType == typeof(UtcDateTime);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
            if (value is UtcDateTime utcDateTime) {
                writer.WriteValue(utcDateTime.ToDateTime());
            }
            else {
                writer.WriteNull();
            }
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
            if (reader.TokenType == JsonToken.Null) {
                return null;
            }

            if (reader.TokenType == JsonToken.Date) {
                return UtcDateTime.ConvertFrom((DateTime)reader.Value);
            }

            if (reader.TokenType == JsonToken.String) {
                return UtcDateTime.Parse((string)reader.Value);
            }

            throw new JsonSerializationException($"Unexpected token type: {reader.TokenType}");
        }
    }
}