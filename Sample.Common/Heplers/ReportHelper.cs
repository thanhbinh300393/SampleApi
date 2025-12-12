using Sample.Common.Languages;
using System;
using System.Globalization;

namespace Sample.Common.Heplers
{
    public static class ReportHelper
    {
        public static string ReportingPeriod(short reportingPeriod, DateTime? fromDate, DateTime? toDate)
        {
            var reportingPeriodString = string.Empty;
            switch (reportingPeriod)
            {
                case 0:
                    reportingPeriodString = "Năm " + fromDate?.Year;
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    reportingPeriodString = "Tháng " + reportingPeriod + " năm " + fromDate?.Year;
                    break;
                case 13:
                    reportingPeriodString = "Quý I năm " + fromDate?.Year;
                    break;
                case 14:
                    reportingPeriodString = "Quý II năm " + fromDate?.Year;
                    break;
                case 15:
                    reportingPeriodString = "Quý III năm " + fromDate?.Year;
                    break;
                case 16:
                    reportingPeriodString = "Quý IV năm " + fromDate?.Year;
                    break;
                default:
                    if (fromDate == toDate)
                        reportingPeriodString = "Ngày " + string.Format(CultureInfo.GetCultureInfo("vi-VN"), ((int)fromDate?.Month < 3) ? "{0:dd/MM/yyyy}" : "{0:dd/M/yyyy}", fromDate);
                    else
                        reportingPeriodString = "Từ ngày " + string.Format(CultureInfo.GetCultureInfo("vi-VN"), ((int)fromDate?.Month < 3) ? "{0:dd/MM/yyyy}" : "{0:dd/M/yyyy}", fromDate) + " đến ngày " + string.Format(CultureInfo.GetCultureInfo("vi-VN"), ((int)toDate?.Month < 3) ? "{0:dd/MM/yyyy}" : "{0:dd/M/yyyy}", toDate);
                    break;
            }
            return reportingPeriodString;
        }
        public static string StringYearFromDateToDate(DateTime? fromDate, DateTime? toDate)
        {
            string year = string.Empty;
            int yearFrom = fromDate == null ? 0 : (int)fromDate?.Year;
            int yearTo = toDate == null ? 0 : (int)toDate?.Year;
            var numberYear = yearTo - yearFrom;
            if (numberYear >= 0)
            {
                for (int i = 0; i <= numberYear; i++)
                {
                    if (string.IsNullOrWhiteSpace(year))
                        year = (yearFrom + i).ToString();
                    else
                        year += "," + (yearFrom + i).ToString();
                }
            }
            return year;
        }
        public static string DayMonthYearString(DateTime? date)
        {
            var DatetimeStr = "ngày .... tháng .... năm .....";
            if (date != null)
                DatetimeStr = $"ngày {date.Value.Day.ToString("D2")} tháng {date.Value.Month.ToString((((int)date?.Month < 3) ? "D2" : "D1"))} năm {date.Value.Year}";
            return DatetimeStr;
        }
        public static string CurrencyUnitString(int currencyUnit)
        {
            var currentUnitString = string.Empty;
            switch (currencyUnit)
            {
                case 1:
                    currentUnitString = "Đơn vị tính: đồng";
                    break;
                case 1000:
                    currentUnitString = "Đơn vị tính: nghìn đồng";
                    break;
                case 1000000:
                    currentUnitString = "Đơn vị tính: triệu đồng";
                    break;
                case 1000000000:
                    currentUnitString = "Đơn vị tính: tỷ đồng";
                    break;
                default:
                    currentUnitString = "Đơn vị tính: đồng";
                    break;
            }
            return currentUnitString;
        }
        public static string CurrencyUnitString(string labelUnit, int currencyUnit)
        {
            var currentUnitString = string.Empty;
            switch (currencyUnit)
            {
                case 1:
                    currentUnitString = "đồng";
                    break;
                case 1000:
                    currentUnitString = "nghìn đồng";
                    break;
                case 1000000:
                    currentUnitString = "triệu đồng";
                    break;
                case 1000000000:
                    currentUnitString = "tỷ đồng";
                    break;
                default:
                    currentUnitString = "đồng";
                    break;
            }
            return labelUnit + currentUnitString;
        }
        public static string TextFormatString(int AfterCommaCurrency)
        {
            return "{0:N" + (AfterCommaCurrency).ToString() + "}";
        }

        public static string ConvertNumberToVNDString(double inputNumber)
        {
            try
            {
                string[] placeValues = { "", CommonResource.ThousandUnit,
                               CommonResource.MillionUnit, CommonResource.BillionUnit };
                string[] unitNumbers = { CommonResource.Zero, CommonResource.One, CommonResource.Two, CommonResource.Three, CommonResource.Four,
                               CommonResource.Five,CommonResource.Six, CommonResource.Seven, CommonResource.Eight, CommonResource.Nine };

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
                            result = $"{CommonResource.One} {result}";
                        else
                        {
                            if ((ones == 5) && (tens > 0))
                                result = $"{CommonResource.FiveUnit} {result}";
                            else if (ones > 0)
                                result = unitNumbers[ones] + " " + result;
                        }
                        if (tens < 0)
                            break;
                        else
                        {
                            if ((tens == 0) && (ones > 0)) result = $"{CommonResource.Odd} {result}";
                            if (tens == 1) result = $"{CommonResource.Ten} {result}";
                            if (tens > 1) result = $"{unitNumbers[tens]} {CommonResource.DozenUnit} {result}";
                        }
                        if (hundreds < 0) break;
                        else
                        {
                            if ((hundreds > 0) || (tens > 0) || (ones > 0))
                                result = $"{unitNumbers[hundreds]} {CommonResource.HundredUnit} {result}";
                        }
                        result = " " + result;
                    }
                }
                result = result.Trim();
                if (isNegative) result = "Âm " + result;
                return $"{result} {CommonResource.VNDUnit}";
            }
            catch
            {
                return $"{CommonResource.Zero} {CommonResource.VNDUnit}";
            }
        }

    }
}
