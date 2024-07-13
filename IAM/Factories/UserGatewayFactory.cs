using IAM.Gateways.Users;
using IAM.Models;
using System;
using System.Net.Http;
using System.Text.Json;

namespace IAM.Factories
{
    /// <summary>
    /// Factory for creating a UserGateway
    /// </summary>
    public class UserGatewayFactory : IUserGatewayFactory
    {
        private readonly IHttpClientFactory myHttpClient;
        public UserGatewayFactory(IHttpClientFactory httpClient)
        {
            myHttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Creates a UserGateway
        /// </summary>
        /// <param name="domain">Auth0 domain</param>
        /// <param name="accessToken">Auth0 access token</param>
        /// <returns></returns>
        public IUsersGateway Create(string domain, string accessToken)
        {
            var connectionData = new Auth0ApiConnectionData
            {
                Domain = domain,
                AccessToken = accessToken
            };
            return new UsersGateway(JsonSerializer.Serialize(connectionData), myHttpClient);
        }
    }
}
