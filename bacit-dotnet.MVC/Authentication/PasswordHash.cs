using System.Security.Cryptography;


namespace bacit_dotnet.MVC.Authentication
{
    public static class PasswordHash
    {
        
        private const int SaltSize = 32;
        
        public static byte[] GenerateSalt()
        { 
            return RandomNumberGenerator.GetBytes(32); ;
        }

        public static byte[] ComputeHMAC_SHA256(byte[] data, byte[] salt)
        {
            using (var hmac = new HMACSHA256(salt))
            {
                return hmac.ComputeHash(data);
            }
        }
    }

}