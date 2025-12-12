using System;

namespace Sample.Common.Extentions
{
    public static class DateTimeExtension
    {
        public static string ToVietnameseFullTime(this DateTime date, bool upperFirstChar = false)
        {
            if (upperFirstChar)
            {
                return $"Ngày {date.Day} tháng {date.Month} năm {date.Year}";
            }
            return $"ngày {date.Day} tháng {date.Month} năm {date.Year}";
        }

        public static string ToVietnameseFullTime(this DateTime? date, bool upperFirstChar = false)
        {
            var dateValue = date ?? DateTime.MinValue;
            if (upperFirstChar)
            {
                return date == null ? "Ngày...tháng...năm..." : $"Ngày {dateValue.Day} tháng {dateValue.Month} năm {dateValue.Year}";
            }
            return date == null ? "ngày...tháng...năm..." : $"ngày {dateValue.Day} tháng {dateValue.Month} năm {dateValue.Year}";
        }
    }
}
