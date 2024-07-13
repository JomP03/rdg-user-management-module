using Microsoft.AspNetCore.Authorization;
using System;


namespace Services.API.Extensions
{
    /// <summary>
    /// Requirement for the scope
    /// </summary>
    public class HasScopeRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Issuer of the Scope
        /// </summary>
        public string Issuer { get; }

        /// <summary>
        /// Scope
        /// </summary>
        public string Scope { get; }


        /// <summary>
        /// Requirement for the scope
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="issuer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public HasScopeRequirement(string scope, string issuer)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }
    }
}