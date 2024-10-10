using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace BreadPack.DateTimeTypes {
    public struct UtcDateTime : IComparable<UtcDateTime>
                              , IEquatable<UtcDateTime>
                              , IComparable
                              , ISerializable {
        private DateTime _dateTime;
        private DateTime DateTimeOrDefault {
            get {
                if (_dateTime.Kind != DateTimeKind.Utc) {
                    _dateTime = MinValue.ToDateTime();
                }

                return _dateTime;
            }
        }

        public static UtcDateTime Now      => new(DateTime.UtcNow);
        public static UtcDateTime MaxValue => new(DateTimeOffset.MaxValue.UtcDateTime);
        public static UtcDateTime MinValue => new(DateTimeOffset.MinValue.UtcDateTime);
        
        private UtcDateTime(DateTime dateTime) {
            if (dateTime == DateTime.MinValue)
                dateTime = DateTimeOffset.MinValue.UtcDateTime;
            if (dateTime == DateTime.MaxValue)
                dateTime = DateTimeOffset.MaxValue.UtcDateTime;
            if (dateTime.Kind != DateTimeKind.Utc) {
                throw new ArgumentException("DateTime must be Utc", nameof(dateTime));
            }

            _dateTime = dateTime;
        }

        public UtcDateTime(SerializationInfo info, StreamingContext context) {
            _dateTime = DateTime.FromBinary(info.GetInt64(nameof(_dateTime)));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(nameof(_dateTime), _dateTime.ToBinary());
        }

        public UtcDateTime(int year, int month, int day, int hour, int minute, int second)
            : this(new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc)) { }

        public UtcDateTime(int year, int month, int day)
            : this(new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc)) { }

        public UtcDateTime(long ticks)
            : this(new DateTime(ticks, DateTimeKind.Utc)) { }

        public int         Year        => DateTimeOrDefault.Year;
        public int         Month       => DateTimeOrDefault.Month;
        public int         Day         => DateTimeOrDefault.Day;
        public int         Hour        => DateTimeOrDefault.Hour;
        public int         Minute      => DateTimeOrDefault.Minute;
        public int         Second      => DateTimeOrDefault.Second;
        public int         Millisecond => DateTimeOrDefault.Millisecond;
        public DayOfWeek   DayOfWeek   => DateTimeOrDefault.DayOfWeek;
        public TimeSpan    TimeOfDay   => DateTimeOrDefault.TimeOfDay;
        public UtcDateTime Date        => new(DateTimeOrDefault.Date);
        public int         DayOfYear   => DateTimeOrDefault.DayOfYear;

        public DateTime ToDateTime() => DateTimeOrDefault;

        public DateTime ToLocalTime(TimeZoneInfo? timeZoneInfo = null) {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTimeOrDefault, timeZoneInfo ?? TimeZoneInfo.Local);
        }

        public long ToBinary() {
            return DateTimeOrDefault.ToBinary();
        }

        public static UtcDateTime FromBinary(long binary) {
            return new(DateTime.FromBinary(binary));
        }

        public override string ToString() => DateTimeOrDefault.ToString(CultureInfo.InvariantCulture);

        public int CompareTo(object obj) {
            if (obj is UtcDateTime other) {
                return DateTimeOrDefault.CompareTo(other.DateTimeOrDefault);
            }
            else {
                throw new ArgumentException("Object is not a UtcDateTime");
            }
        }

        public override int GetHashCode() => DateTimeOrDefault.GetHashCode();

        public int CompareTo(UtcDateTime other) {
            return DateTimeOrDefault.CompareTo(other.DateTimeOrDefault);
        }

        public bool Equals(UtcDateTime other) {
            return DateTimeOrDefault.Equals(other.DateTimeOrDefault);
        }

        public override bool Equals(object obj) => obj is UtcDateTime other && DateTimeOrDefault.Equals(other.DateTimeOrDefault);

        public static UtcDateTime Parse(string s) => Parse(s, CultureInfo.InvariantCulture);

        public static UtcDateTime Parse(string s, IFormatProvider provider) {
            if (!DateTime.TryParse(s, provider, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var result)) {
                throw new FormatException("Invalid date format.");
            }

            if (result.Kind != DateTimeKind.Utc) {
                throw new ArgumentException("Provided date string is not in UTC format.");
            }

            return new(result);
        }

        public static bool TryParse(string? s, out UtcDateTime result) => TryParse(s, CultureInfo.InvariantCulture, out result);

        public static bool TryParse(string? s, IFormatProvider? provider, out UtcDateTime result) {
            if (DateTime.TryParse(s, provider, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var dtResult)) {
                if (dtResult.Kind == DateTimeKind.Utc) {
                    result = new(dtResult);
                    return true;
                }
            }

            result = default;
            return false;
        }

        public static UtcDateTime ConvertFrom(TimeZoneInfo timezone, DateTime dateTime) {
            if (dateTime.Kind == DateTimeKind.Utc)
                return new(dateTime);
            return new(TimeZoneInfo.ConvertTimeToUtc(dateTime, timezone));
        }

        public static UtcDateTime ConvertFrom(DateTime dateTime) {
            return new(dateTime);
        }

        public static bool operator <(UtcDateTime  a, UtcDateTime b) => a.DateTimeOrDefault < b.DateTimeOrDefault;
        public static bool operator >(UtcDateTime  a, UtcDateTime b) => a.DateTimeOrDefault > b.DateTimeOrDefault;
        public static bool operator <=(UtcDateTime a, UtcDateTime b) => a.DateTimeOrDefault <= b.DateTimeOrDefault;
        public static bool operator >=(UtcDateTime a, UtcDateTime b) => a.DateTimeOrDefault >= b.DateTimeOrDefault;
        public static bool operator ==(UtcDateTime a, UtcDateTime b) => a.DateTimeOrDefault == b.DateTimeOrDefault;
        public static bool operator !=(UtcDateTime a, UtcDateTime b) => a.DateTimeOrDefault != b.DateTimeOrDefault;

        public static TimeSpan operator -(UtcDateTime a, UtcDateTime b) => a.DateTimeOrDefault - b.DateTimeOrDefault;

        public static UtcDateTime operator +(UtcDateTime a, TimeSpan b) => new(a.DateTimeOrDefault + b);
        public static UtcDateTime operator -(UtcDateTime a, TimeSpan b) => new(a.DateTimeOrDefault - b);

        public static int Compare(UtcDateTime lastDisconnectDate, UtcDateTime lastConnectDate) {
            return lastDisconnectDate.DateTimeOrDefault.CompareTo(lastConnectDate.DateTimeOrDefault);
        }
    }
}