using Domain.Shared;
using System;

namespace Domain.Entities.Users
{
    public class UserName : ValueOject
    {
        private readonly int maxNameLength = int.Parse(Environment.GetEnvironmentVariable("MAX_NAME_LENGTH"));
        private readonly int minNameLength = 2;
        public string Value { get; private set; }

        protected UserName()
        {
            // Required by EF!!!
        }

        public UserName(string name)
        {

            // Check if name is null or empty
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            // Check if name is longer than the max length
            if (name.Length > maxNameLength || name.Length < minNameLength)
            {
                throw new BusinessRuleValidationException($"Name must have between {minNameLength} and {maxNameLength} characters");
            }

            // Check if the name is only contains letters and spaces
            if (!Guard.ContainsOnlyLettersAndSpaces(name))
            {
                throw new BusinessRuleValidationException("Name must contain only contain letters and spaces");
            }
            Value = name;
        }
    }
}