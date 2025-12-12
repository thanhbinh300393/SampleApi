using System;
using System.Security.Cryptography;
using System.Text;

namespace Sample.Common.Security
{
    public static class CryptographyHelper
    {
        private static HashAlgorithm Create(CryptographyAlgorithms algorithm)
        {
            switch (algorithm)
            {
                case CryptographyAlgorithms.SHA256:
                    return SHA256.Create();
                case CryptographyAlgorithms.SHA512:
                    return SHA512.Create();
                case CryptographyAlgorithms.MD5:
                default:
                    return MD5.Create();
            }
        }
        public static string GetHash(string input, CryptographyAlgorithms algorithm)
        {
            using (var hashTooler = Create(algorithm))
            {
                // Convert the input string to a byte array and compute the hash. 
                var data = hashTooler.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes 
                // and create a string.
                var builder = new StringBuilder();

                // Loop through each byte of the hashed data  
                // and format each one as a hexadecimal string. 
                foreach (byte t in data)
                {
                    builder.Append(t.ToString("x2"));
                }

                // Return the hexadecimal string. 
                return builder.ToString();
            }

        }

        public static bool VerifyHash(string input, string hash, CryptographyAlgorithms algorithm)
        {
            // Hash the input. 
            var hashOfInput = GetHash(input, algorithm);

            // Create a StringComparer an compare the hashes.
            var comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }
    }

    public enum CryptographyAlgorithms
    {
        MD5,
        SHA256,
        SHA512
    }
}
