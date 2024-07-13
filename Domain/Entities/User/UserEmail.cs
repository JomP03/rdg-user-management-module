using Domain.Shared;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Domain.Entities.Users
{
    public class UserEmail : ValueOject
    {
        private readonly Regex emailRegex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        public string Value { get; private set; }

        private readonly string[] allowedDomains = Environment.GetEnvironmentVariable("ALLOWED_DOMAINS").Split(',');

        protected UserEmail()
        {
            // Required by EF!!!
        }

        public UserEmail(string email)
        {

            // Check if email is null or empty
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            // Regex to check if email is valid
            if (!emailRegex.IsMatch(email))
            {
                throw new BusinessRuleValidationException("Email format is invalid!");
            }

            // Check if email contains a valid domain
            string[] emailParts = email.Split("@");
            if (!allowedDomains.Contains(emailParts[1]))
            {
                throw new BusinessRuleValidationException("Email must contain a valid domain");
            }

            Value = email;
        }
    }
}