using HtmlAgilityPack;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using static System.String;

namespace Sample.Common.Heplers
{
    public static class StringHelper
    {
        public static string UrlFriendly(this string text, int maxLength = 0)
        {
            // Return empty value if text is null
            if (text == null) return "";

            var normalizedString = text
                // Make lowercase
                .ToLowerInvariant()
                // Normalize the text
                .Normalize(NormalizationForm.FormD);

            var stringBuilder = new StringBuilder();
            var stringLength = normalizedString.Length;
            var prevdash = false;
            var trueLength = 0;

            char c;

            for (int i = 0; i < stringLength; i++)
            {
                c = normalizedString[i];

                switch (CharUnicodeInfo.GetUnicodeCategory(c))
                {
                    // Check if the character is a letter or a digit if the character is a
                    // international character remap it to an ascii valid character
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        if (c < 128)
                            stringBuilder.Append(c);
                        else
                            stringBuilder.Append(RemapInternationalCharToAscii(c));

                        prevdash = false;
                        trueLength = stringBuilder.Length;
                        break;

                    // Check if the character is to be replaced by a hyphen but only if the last character wasn't
                    case UnicodeCategory.SpaceSeparator:
                    case UnicodeCategory.ConnectorPunctuation:
                    case UnicodeCategory.DashPunctuation:
                    case UnicodeCategory.OtherPunctuation:
                    case UnicodeCategory.MathSymbol:
                        if (!prevdash)
                        {
                            stringBuilder.Append('-');
                            prevdash = true;
                            trueLength = stringBuilder.Length;
                        }

                        break;
                }

                // If we are at max length, stop parsing
                if (maxLength > 0 && trueLength >= maxLength)
                    break;
            }

            // Trim excess hyphens
            var result = stringBuilder.ToString().Trim('-');

            // Remove any excess character to meet maxlength criteria
            return maxLength <= 0 || result.Length <= maxLength ? result : result.Substring(0, maxLength);
        }

        public static string IsNotNullOrWhiteSpace(this string str, string valueReturnIfNotNull)
        {
            if (IsNullOrWhiteSpace(str))
                return "";

            return valueReturnIfNotNull;
        }

        public static string ToLowerFirstChar(this string str)
        {
            if (IsNullOrWhiteSpace(str))
                return "";

            return str?.ToLowerInvariant()[0] + ((str.Length > 1) ? str.Substring(1) : "");
        }

        public static string ToUpperFirstChar(this string str)
        {
            if (IsNullOrWhiteSpace(str))
                return "";

            return str?.ToUpperInvariant()[0] + ((str.Length > 1) ? str.Substring(1) : "");
        }

        public static string RemoveDuplicateSpace(this string str)
        {
            while (str != str?.Replace("  ", " "))
                str = str?.Replace("  ", " ");
            return str;
        }

        public static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }

        public static string RemoveMutilSpaces(string str)
        {
            if (IsNullOrEmpty(str))
                return Empty;
            str = str.Trim();
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            str = regex.Replace(str, " ");
            return str;
        }

        public static string GetMomentSuffixes(this DateTime moment)
        {
            var result = $"{moment.Year}{moment.Month}{moment.Day}" +
                         $"{moment.Hour}{moment.Minute}{moment.Second}";

            return result;
        }

        public static string GetEnumDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }

        public static string ConvertHtmlToPlainText(this string str)
        {
            if (IsNullOrWhiteSpace(str))
                return Empty;
            var doc = new HtmlDocument();
            str = HttpUtility.UrlDecode(str);
            str = HttpUtility.HtmlDecode(str);
            doc.LoadHtml(str);

            return doc.DocumentNode.InnerText;
        }

        public static string RemoveSpanTagFromHtml(this string str)
        {
            if (IsNullOrWhiteSpace(str))
            {
                return Empty;
            }

            str = Regex.Replace(str, @"</?span( [^>]*|/)?>", Empty);
            return str;
        }

        public static string IntToRoman(this int num)
        {
            string romanResult = Empty;
            string[] romanLetters =
            {
                "M",
                "CM",
                "D",
                "CD",
                "C",
                "XC",
                "L",
                "XL",
                "X",
                "IX",
                "V",
                "IV",
                "I"
            };
            int[] numbers =
            {
                1000,
                900,
                500,
                400,
                100,
                90,
                50,
                40,
                10,
                9,
                5,
                4,
                1
            };
            int i = 0;
            while (num != 0)
            {
                if (num >= numbers[i])
                {
                    num -= numbers[i];
                    romanResult += romanLetters[i];
                }
                else
                {
                    i++;
                }
            }

            return romanResult;
        }

        public static string RemoveSpecialCharacterAndNormalize(this string input)
        {
            return input
                .Replace("-", "")
                .Replace(" ", "")
                .Normalize()
                .Trim();
        }

        public static string RemoveDuplicateSpaceAndNormalize(this string input)
        {
            return input
                .Replace("  ", " ")
                .Trim()
                .Normalize();
        }

        public static string RemoveDuplicateSpaceAndNormalizeLower(this string input)
        {
            return input
                .Replace("  ", " ")
                .Trim()
                .Normalize()
                .ToLower();
        }
    }
}