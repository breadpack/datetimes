using Newtonsoft.Json;

namespace BreadPack.DateTimeTypes.NewtonsoftJson {
    public static class UtcDateTimeRegister {
        public static JsonSerializerSettings UseUtcDateTime(this JsonSerializerSettings settings) {
            settings.Converters.Add(new UtcDateTimeJsonConverter());
            return settings;
        }
    }
}