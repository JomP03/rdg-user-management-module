using System;

namespace IAM.Exceptions
{
    /// <summary>
    /// Exception for when a Auth0 returns a 429 Too Many Requests.
    /// </summary>
    public class Auth0TooManyRequestsException : Exception
    {
        /// <summary>
        /// Constructor for the exception.
        /// </summary>
        /// <param name="message"></param>
        public Auth0TooManyRequestsException(string message) : base(message)
        {
        }

    }
}
