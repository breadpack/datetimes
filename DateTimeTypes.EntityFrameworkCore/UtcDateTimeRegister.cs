using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using BreadPack.DateTimeTypes;

namespace BreadPack.DateTimeTypes.EntityFrameworkCore {
    public static class UtcDateTimeRegister {
        public static void UseUtcDateTimeForNpgsql(this ModelConfigurationBuilder configurationBuilder) {
            configurationBuilder
                .Properties<UtcDateTime>()
                .HaveConversion<UtcDateTimeConverter>();

            configurationBuilder
                .DefaultTypeMapping<UtcDateTime>()
                .HasConversion<UtcDateTimeConverter>();
        }

        public static void UseUtcDateTime(this ModelBuilder modelBuilder) {
            modelBuilder.RegistAddFunction(typeof(UtcDateTimeExt).GetMethod(nameof(UtcDateTimeExt.AddSeconds))!, "second");
            modelBuilder.RegistAddFunction(typeof(UtcDateTimeExt).GetMethod(nameof(UtcDateTimeExt.AddMinutes))!, "minute");
            modelBuilder.RegistAddFunction(typeof(UtcDateTimeExt).GetMethod(nameof(UtcDateTimeExt.AddHours))!, "hour");
            modelBuilder.RegistAddFunction(typeof(UtcDateTimeExt).GetMethod(nameof(UtcDateTimeExt.AddDays))!, "day");
            modelBuilder.RegistAddFunction(typeof(UtcDateTimeExt).GetMethod(nameof(UtcDateTimeExt.AddMonths))!, "month");
            modelBuilder.RegistAddFunction(typeof(UtcDateTimeExt).GetMethod(nameof(UtcDateTimeExt.AddYears))!, "year");
        }

        private static DbFunctionBuilder RegistAddFunction(this ModelBuilder modelBuilder, MethodInfo methodInfo, string timeUnit) {
            return modelBuilder
                   .HasDbFunction(methodInfo)
                   .HasTranslation(args => {
                       // Your translation logic goes here.
                       // This example assumes a SQL function 'AddSecondsToDateTime' exists.
                       var utcDateTimeExpression = args.First();
                       var secondsExpression     = args.Skip(1).First();

                       var paramExpression = new SqlBinaryExpression(
                           ExpressionType.Multiply,
                           new SqlFragmentExpression($"interval '1 {timeUnit}'")
                         , secondsExpression
                         , typeof(DateTime)
                         , null);

                       return new SqlBinaryExpression(
                           ExpressionType.Add,
                           utcDateTimeExpression,
                           paramExpression,
                           utcDateTimeExpression.Type,
                           null);
                   });
        }

        private static bool IsNullable(this Type type) {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}