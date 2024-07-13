using System;

namespace IAM.Exceptions
{
    /// <summary>
    /// Exception for when a Auth0 returns a 404 Not Found.
    /// </summary>
    public class Auth0UserNotFoundException : Exception
    {
        /// <summary>
        /// Constructor for the exception.
        /// </summary>
        /// <param name="message"></param>
        public Auth0UserNotFoundException(string message) : base(message)
        {
        }

    }
}
