using IAM.Gateways.Users;

namespace IAM.Factories
{
    /// <summary>
    /// Interface for the UserGatewayFactory
    /// </summary>
    public interface IUserGatewayFactory
    {
        /// <summary>
        /// Creates a UserGateway
        /// </summary>
        /// <param name="domain">Auth0 tenant domain</param>
        /// <param name="acccessToken">Auth0 tenant Access token</param>
        /// <returns></returns>
        IUsersGateway Create(string domain, string acccessToken);
    }
}
