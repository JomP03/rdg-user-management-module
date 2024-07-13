using Domain.Shared;
using System;
using System.Linq;

namespace Domain.Entities.Users
{
    public class UserPhoneNumber
    {
        public string Value { get; private set; }

        private readonly string[] allowedFormats = { @"^(9|3519)[1236]\d{7}$" };

        protected UserPhoneNumber()
        {
            // Required by EF!!!
        }

        // This is a very simple implementation of a phone number validation, in a real world cenario there should be validators
        // for each country
        public UserPhoneNumber(string phoneNumber)
        {
            // Check if phoneNumber is null or empty
            if (string.IsNullOrEmpty(phoneNumber))
            {
                throw new ArgumentNullException(nameof(phoneNumber));
            }

            // Check if the phoneNumber is valid - has to match one of the allowed formats
            if (!allowedFormats.Any(format => IsValidFormat(phoneNumber, format)))
            {
                throw new BusinessRuleValidationException("Phone number must be a valid portuguese phone number");
            }

            Value = phoneNumber;
        }

        private static bool IsValidFormat(string phoneNumber, string format)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, format);
        }
    }
}
