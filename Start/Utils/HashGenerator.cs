using System.Security.Cryptography;
using System.Text;

namespace Start.Common.Utils
{
    public class HashGenerator
    {
        public static string GenerateHash(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }

            var sha1 = SHA1.Create();
            var data = Encoding.ASCII.GetBytes(password);
            var sha1data = sha1.ComputeHash(data);
            var hashedPassword = Encoding.ASCII.GetString(sha1data);
            return hashedPassword;
        }
        
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            var sha1 = SHA1.Create();
            var data = Encoding.ASCII.GetBytes(password);
            var sha1data = sha1.ComputeHash(data);
            var hashedCheckPassword = Encoding.ASCII.GetString(sha1data);

            return hashedCheckPassword == hashedPassword;
        }

    }
}
