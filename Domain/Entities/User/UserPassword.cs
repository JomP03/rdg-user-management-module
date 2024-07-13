using Domain.Shared;

namespace Domain.Entities.Users
{
    public class UserPassword
    {
        public string Value { get; private set; }

        protected UserPassword()
        {
            // Required by EF!!!
        }

        public UserPassword(string password)
        {
            if (!PasswordPolicy.IsValidPassword(password))
            {
                throw new BusinessRuleValidationException("Password does not meet the password policy");
            }
            Value = password;
        }
    }
}
