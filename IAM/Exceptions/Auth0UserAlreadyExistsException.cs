using System;

namespace IAM.Exceptions
{
    /// <summary>
    /// Exception for when a Auth0 returns a 409 Conflict.
    /// </summary>
    public class Auth0UserAlreadyExistsException : Exception
    {
        /// <summary>
        /// Constructor for the exception.
        /// </summary>
        /// <param name="message"></param>
        public Auth0UserAlreadyExistsException(string message) : base(message)
        {
        }

    }
}
