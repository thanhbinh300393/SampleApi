using Sample.Common.Heplers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sample.Common.Extentions
{
    public static class ConvertHepler
    {
        public static long ToLong(this object obj, long defaultValue = 0)
        {
            return obj.ToLongOrNull() ?? defaultValue;
        }

        public static long? ToLongOrNull(this object obj, long? defaultValue = null)
        {
            try
            {
                string str = obj?.ToString() ?? "";
                return Int64.Parse(str);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static int ToInt(this object obj, int defaultValue = 0)
        {
            return obj.ToIntOrNull() ?? defaultValue;
        }

        public static int? ToIntOrNull(this object obj, int? defaultValue = null)
        {
            try
            {
                string str = obj?.ToString() ?? "";
                return Int32.Parse(str);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static byte ToByte(this object obj, byte defaultValue = 0)
        {
            return obj.ToByteOrNull() ?? defaultValue;
        }

        public static byte? ToByteOrNull(this object obj, byte? defaultValue = null)
        {
            try
            {
                string str = obj?.ToString() ?? "";
                return Byte.Parse(str);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static short ToShort(this object obj, short defaultValue = 0)
        {
            return obj.ToShortOrNull() ?? defaultValue;
        }

        public static short? ToShortOrNull(this object obj, short? defaultValue = null)
        {
            try
            {
                string str = obj?.ToString() ?? "";
                return Int16.Parse(str);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static bool ToBool(this object obj, bool defaultValue = false)
        {
            return obj.ToBoolOrNull() ?? defaultValue;
        }

        public static bool? ToBoolOrNull(this object obj, bool? defaultValue = null)
        {
            try
            {
                string str = obj?.ToString() ?? "";
                return str == "true" || str == "True" || str.ToLong() > 0;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static Guid ToGuid(this object obj, Guid defaultValue = new Guid())
        {
            return obj.ToGuidOrNull() ?? defaultValue;
        }

        public static Guid? ToGuidOrNull(this object obj, Guid? defaultValue = null)
        {
            try
            {
                var str = obj?.ToString() ?? "";
                return new Guid(str);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static DateTime ToDateTime(this object obj, DateTime defaultValue = new DateTime())
        {
            return obj.ToDateTimeOrNull() ?? defaultValue;
        }

        public static DateTime? ToDateTimeOrNull(this object obj, DateTime? defaultValue = null)
        {
            try
            {
                var str = obj?.ToString() ?? "";
                return DateTime.Parse(str);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static TResult ToMapper<TResult>(this object obj) where TResult : class
        {
            return MapHelper.Mapper<object, TResult>(obj);
        }

        public static T ConverToObject<T>(this string value, T defaultValue)
        {
            if (typeof(T) == typeof(bool))
                return (T)(object)value.ToBool((bool)(object)defaultValue);
            if (typeof(T) == typeof(bool?))
                return (T)(object)value.ToBoolOrNull((bool?)(object)defaultValue);

            if (typeof(T) == typeof(short))
                return (T)(object)value.ToShort((short)(object)defaultValue);
            if (typeof(T) == typeof(short?))
                return (T)(object)value.ToShortOrNull((short?)(object)defaultValue);

            if (typeof(T) == typeof(int))
                return (T)(object)value.ToInt((int)(object)defaultValue);
            if (typeof(T) == typeof(int?))
                return (T)(object)value.ToIntOrNull((int?)(object)defaultValue);

            if (typeof(T) == typeof(long))
                return (T)(object)value.ToLong((long)(object)defaultValue);
            if (typeof(T) == typeof(long?))
                return (T)(object)value.ToLongOrNull((long?)(object)defaultValue);

            if (typeof(T) == typeof(DateTime))
                return (T)(object)value.ToDateTime((DateTime)(object)defaultValue);
            if (typeof(T) == typeof(DateTime?))
                return (T)(object)value.ToDateTimeOrNull((DateTime?)(object)defaultValue);

            if (typeof(T) == typeof(Guid))
                return (T)(object)value.ToGuid((Guid)(object)defaultValue);
            if (typeof(T) == typeof(Guid?))
                return (T)(object)value.ToGuidOrNull((Guid?)(object)defaultValue);

            try
            {
                if (typeof(T) == typeof(string))
                    return (T)(object)value.ToString();
            }
            catch (Exception)
            {
                return defaultValue;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static string NumberToText(double inputNumber, bool suffix = true)
        {
            string[] unitNumbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = new string[] { "", "nghìn", "triệu", "tỷ" };
            bool isNegative = false;

            // -12345678.3445435 => "-12345678"
            string sNumber = inputNumber.ToString("#");
            double number = Convert.ToDouble(sNumber);
            if (number < 0)
            {
                number = -number;
                sNumber = number.ToString();
                isNegative = true;
            }


            int ones, tens, hundreds;

            int positionDigit = sNumber.Length;   // last -> first

            string result = " ";


            if (positionDigit == 0)
                result = unitNumbers[0] + result;
            else
            {
                // 0:       ###
                // 1: nghìn ###,###
                // 2: triệu ###,###,###
                // 3: tỷ    ###,###,###,###
                int placeValue = 0;

                while (positionDigit > 0)
                {
                    // Check last 3 digits remain ### (hundreds tens ones)
                    tens = hundreds = -1;
                    ones = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                    positionDigit--;
                    if (positionDigit > 0)
                    {
                        tens = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                        positionDigit--;
                        if (positionDigit > 0)
                        {
                            hundreds = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                            positionDigit--;
                        }
                    }

                    if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                        result = placeValues[placeValue] + result;

                    placeValue++;
                    if (placeValue > 3) placeValue = 1;

                    if ((ones == 1) && (tens > 1))
                        result = "một " + result;
                    else
                    {
                        if ((ones == 5) && (tens > 0))
                            result = "lăm " + result;
                        else if (ones > 0)
                            result = unitNumbers[ones] + " " + result;
                    }
                    if (tens < 0)
                        break;
                    else
                    {
                        if ((tens == 0) && (ones > 0)) result = "lẻ " + result;
                        if (tens == 1) result = "mười " + result;
                        if (tens > 1) result = unitNumbers[tens] + " mươi " + result;
                    }
                    if (hundreds < 0) break;
                    else
                    {
                        if ((hundreds > 0) || (tens > 0) || (ones > 0))
                            result = unitNumbers[hundreds] + " trăm " + result;
                    }
                    result = " " + result;
                }
            }
            result = result.Trim();
            if (isNegative) result = "Âm " + result;
            return result + (suffix ? " đồng chẵn" : "");
        }

    }
}


