using System;
using Starter.DateTimeTypes;

public static class UtcDateTimeExt {
    public static UtcDateTime AddSeconds(this UtcDateTime utcDateTime, double duration) {
        return utcDateTime + TimeSpan.FromSeconds(duration);
    }

    public static UtcDateTime AddMinutes(this UtcDateTime utcDateTime, double duration) {
        return utcDateTime + TimeSpan.FromMinutes(duration);
    }

    public static UtcDateTime AddHours(this UtcDateTime utcDateTime, double duration) {
        return utcDateTime + TimeSpan.FromHours(duration);
    }

    public static UtcDateTime AddDays(this UtcDateTime utcDateTime, double duration) {
        return utcDateTime + TimeSpan.FromDays(duration);
    }

    public static UtcDateTime AddMonths(this UtcDateTime utcDateTime, int duration) {
        return UtcDateTime.ConvertFrom(utcDateTime.ToDateTime().AddMonths(duration));
    }

    public static UtcDateTime AddYears(this UtcDateTime utcDateTime, int duration) {
        return UtcDateTime.ConvertFrom(utcDateTime.ToDateTime().AddYears(duration));
    }

    public static TimeSpan Subtract(this UtcDateTime utcDateTime, UtcDateTime utc) {
        return utcDateTime - utc;
    }
}