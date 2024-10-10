using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace BreadPack.DateTimeTypes {
    public struct UtcDateTime : IComparable<UtcDateTime>
                              , IEquatable<UtcDateTime>
                              , IComparable
                              , ISerializable {
        private DateTime _dateTime;

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

        public int         Year        => _dateTime.Year;
        public int         Month       => _dateTime.Month;
        public int         Day         => _dateTime.Day;
        public int         Hour        => _dateTime.Hour;
        public int         Minute      => _dateTime.Minute;
        public int         Second      => _dateTime.Second;
        public int         Millisecond => _dateTime.Millisecond;
        public DayOfWeek   DayOfWeek   => _dateTime.DayOfWeek;
        public TimeSpan    TimeOfDay   => _dateTime.TimeOfDay;
        public UtcDateTime Date        => new(_dateTime.Date);
        public int         DayOfYear   => _dateTime.DayOfYear;

        public DateTime ToDateTime() => _dateTime;

        public DateTime ToLocalTime(TimeZoneInfo? timeZoneInfo = null) {
            return TimeZoneInfo.ConvertTimeFromUtc(_dateTime, timeZoneInfo ?? TimeZoneInfo.Local);
        }

        public long ToBinary() {
            return _dateTime.ToBinary();
        }

        public static UtcDateTime FromBinary(long binary) {
            return new(DateTime.FromBinary(binary));
        }

        public override string ToString() => _dateTime.ToString(CultureInfo.InvariantCulture);

        public int CompareTo(object obj) {
            if (obj is UtcDateTime other) {
                return _dateTime.CompareTo(other._dateTime);
            }
            else {
                throw new ArgumentException("Object is not a UtcDateTime");
            }
        }

        public override int GetHashCode() => _dateTime.GetHashCode();

        public int CompareTo(UtcDateTime other) {
            return _dateTime.CompareTo(other._dateTime);
        }

        public bool Equals(UtcDateTime other) {
            return _dateTime.Equals(other._dateTime);
        }

        public override bool Equals(object obj) => obj is UtcDateTime other && _dateTime.Equals(other._dateTime);

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

        public static bool operator <(UtcDateTime  a, UtcDateTime b) => a._dateTime < b._dateTime;
        public static bool operator >(UtcDateTime  a, UtcDateTime b) => a._dateTime > b._dateTime;
        public static bool operator <=(UtcDateTime a, UtcDateTime b) => a._dateTime <= b._dateTime;
        public static bool operator >=(UtcDateTime a, UtcDateTime b) => a._dateTime >= b._dateTime;
        public static bool operator ==(UtcDateTime a, UtcDateTime b) => a._dateTime == b._dateTime;
        public static bool operator !=(UtcDateTime a, UtcDateTime b) => a._dateTime != b._dateTime;

        public static TimeSpan operator -(UtcDateTime a, UtcDateTime b) => a._dateTime - b._dateTime;

        public static UtcDateTime operator +(UtcDateTime a, TimeSpan b) => new(a._dateTime + b);
        public static UtcDateTime operator -(UtcDateTime a, TimeSpan b) => new(a._dateTime - b);

        public static int Compare(UtcDateTime lastDisconnectDate, UtcDateTime lastConnectDate) {
            return lastDisconnectDate._dateTime.CompareTo(lastConnectDate._dateTime);
        }
    }
}