namespace Sample.Common.Heplers
{
    public class OtpHelper
    {
        private static Dictionary<string, (string otp, DateTime expiry)> otpStore = new Dictionary<string, (string, DateTime)>();

        public static string GenerateOTP(string userId, int expiryMinutes = 5)
        {
            Random random = new();
            string otp = random.Next(100000, 999999).ToString();
            DateTime expiryTime = DateTime.Now.AddMinutes(expiryMinutes);
            otpStore[userId] = (otp, expiryTime);
            CleanupExpiredOTPs();
            return otp;
        }

        public static bool VerifyOTP(string userId, string otp)
        {
            if (otpStore.ContainsKey(userId))
            {
                var (storedOtp, expiryTime) = otpStore[userId];
                if (storedOtp == otp && DateTime.Now <= expiryTime)
                {
                    otpStore.Remove(userId);
                    return true;
                }
            }
            return false;
        }

        private static void CleanupExpiredOTPs()
        {
            var expiredKeys = otpStore.Where(kvp => kvp.Value.expiry <= DateTime.Now).Select(kvp => kvp.Key).ToList();
            foreach (var key in expiredKeys)
            {
                otpStore.Remove(key);
            }
        }
    }
}
