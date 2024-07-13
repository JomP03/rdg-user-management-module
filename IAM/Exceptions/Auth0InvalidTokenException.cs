using System;

namespace IAM.Exceptions
{
    /// <summary>
    /// Exception for when a Auth0 returns a 401 Unauthorized.
    /// </summary>
    public class Auth0InvalidTokenException : Exception
    {
        /// <summary>
        /// Constructor for the exception.
        /// </summary>
        /// <param name="message"></param>
        public Auth0InvalidTokenException(string message) : base(message)
        {
        }

    }
}
