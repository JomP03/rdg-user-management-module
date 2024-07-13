using System;

namespace IAM.Exceptions
{
    /// <summary>
    /// Exception for when a Auth0 returns a 400 Bad Request.
    /// </summary>
    public class Auth0BadRequestException : Exception
    {
        /// <summary>
        /// Constructor for the exception.
        /// </summary>
        /// <param name="message"></param>
        public Auth0BadRequestException(string message) : base(message)
        {
        }

    }
}
