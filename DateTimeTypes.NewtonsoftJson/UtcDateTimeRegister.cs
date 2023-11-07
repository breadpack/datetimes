using Newtonsoft.Json;

namespace Starter.DateTypeTypes.NewtonsoftJson {
    public static class UtcDateTimeRegister {
        public static JsonSerializerSettings UseUtcDateTime(this JsonSerializerSettings settings) {
            settings.Converters.Add(new UtcDateTimeJsonConverter());
            return settings;
        }
    }
}