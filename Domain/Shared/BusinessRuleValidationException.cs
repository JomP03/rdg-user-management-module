using System;

namespace Domain.Shared
{
    /// <summary>
    /// Exception that is thrown when a business rule is not satisfied
    /// </summary>
    public class BusinessRuleValidationException : Exception
    {
        public string Details { get; }

        public BusinessRuleValidationException(string message) : base(message)
        {
        }

        public BusinessRuleValidationException(string message, string details) : base(message)
        {
            this.Details = details;
        }
    }
}
