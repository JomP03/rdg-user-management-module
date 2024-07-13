using System;

namespace IAM.Exceptions
{
    /// <summary>
    /// Exception for when a Auth0 returns a 403 Forbidden.
    /// </summary>
    public class Auth0InsufficientScopeException : Exception
    {
        /// <summary>
        /// Constructor for the exception.
        /// </summary>
        /// <param name="message"></param>
        public Auth0InsufficientScopeException(string message) : base(message)
        {
        }

    }
}
