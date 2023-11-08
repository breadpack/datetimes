using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Starter.DateTimeTypes;

namespace Starter.DateTimeTypes.EntityFrameworkCore {
    public class UtcDateTimeConverter : ValueConverter<UtcDateTime, DateTime> {
        public UtcDateTimeConverter() : base(
            utcDateTime => utcDateTime.ToDateTime(),
            dateTime => UtcDateTime.ConvertFrom(dateTime)
        ) { }
    }
}