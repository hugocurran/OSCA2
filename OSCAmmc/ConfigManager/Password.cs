using System.Linq;
using System.Security.Cryptography;

namespace OSCASnapin
{
    class Password
    {
        private static string salt = "OSCAmmc";

        internal static byte[] hashPassword(string password)
        {
            using (SHA256CryptoServiceProvider sha = new SHA256CryptoServiceProvider())
            {
                return sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(salt + password));
            }
        }

        internal static bool ValidatePassword(byte[] hash, string password)
        {
            byte[] computedHash = hashPassword(password);

            if (hash.SequenceEqual<byte>(computedHash) == true)
                return true;
            else
                return false;
        }
    }
}
