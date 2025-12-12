using System.Collections.Generic;
using System.Linq;

namespace Sample.Common.Extentions
{
    public static class StringExtension
    {
        public static string RemoveIndexing(this string request)
        {
            var firstDotIndex = request.IndexOf('.');
            if (firstDotIndex == -1)
                return request;

            return request.Substring(firstDotIndex + 1).Trim();
        }

        public static List<string> BreakBySemiColon(this string request)
        {
            if (string.IsNullOrEmpty(request))
            {
                return new List<string>();
            }

            var lines = request.Split(";").ToList();
            for (var i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i].Trim();
            }
            return lines;
        }
    }
}
