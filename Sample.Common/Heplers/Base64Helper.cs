namespace Sample.Common.Heplers
{
    public static class Base64Helper
    {
        public static string ToBase64String(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string DecodeBase64String(this string base64Encoded)
        {
            byte[] data = System.Convert.FromBase64String(base64Encoded);
            return System.Text.ASCIIEncoding.ASCII.GetString(data);
        }

    }
}
