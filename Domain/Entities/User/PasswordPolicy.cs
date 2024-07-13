using System.Linq;

namespace Domain.Entities.Users
{
    /// <summary>
    /// Password policy class
    /// </summary>
    internal class PasswordPolicy
    {
        private static readonly int MIN_LENGTH = 10;
        public static bool IsValidPassword(string password)
        {
            // Check minimum length
            if (password.Length < MIN_LENGTH)
            {
                return false;
            }

            // Check for at least one uppercase letter
            if (!password.Any(char.IsUpper))
            {
                return false;
            }

            // Check for at least one lowercase letter
            if (!password.Any(char.IsLower))
            {
                return false;
            }

            // Check for at least one digit
            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            // Check for at least one symbol
            if (!password.Any(IsSymbol))
            {
                return false;
            }

            return true;
        }

        private static bool IsSymbol(char c)
        {
            // You can customize the symbols as needed
            string symbols = "!@#$%^&*()-_=+[]{}|;:'\"<>,.?/";
            return symbols.Contains(c);
        }
    }
}
