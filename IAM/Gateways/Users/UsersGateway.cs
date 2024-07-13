using IAM.Exceptions;
using IAM.Models;
using IAM.Policies;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace IAM.Gateways.Users
{
    /// <summary>
    /// Gateway for interacting with the Auth0 Users API.
    /// </summary>
    public class UsersGateway : IUsersGateway
    {
        private readonly Auth0ApiConnectionData myConnectionData;
        private readonly IHttpClientFactory myHttpClientFactory;
        private readonly HttpClient myHttpClient;
        private readonly JsonSerializerOptions auth0JsonSerializerOptions = new()
        {
            PropertyNamingPolicy = new Auth0JsonNamingPolicy(),
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        /// <summary>
        /// Creates a new instance of the UsersGateway.
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="httpClientFactory"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UsersGateway(string connectionData, IHttpClientFactory httpClientFactory)
        {
            if (string.IsNullOrEmpty(connectionData))
            {
                throw new ArgumentNullException(nameof(connectionData));
            }
            myConnectionData = JsonSerializer.Deserialize<Auth0ApiConnectionData>(connectionData);
            myHttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            myHttpClient = myHttpClientFactory.CreateClient();
            myHttpClient.BaseAddress = new Uri(myConnectionData.Domain);
            myHttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {myConnectionData.AccessToken}");
        }

        /// <summary>
        /// Creates a User in Auth0.
        /// </summary>
        /// <param name="pUser"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Auth0UserResponse> CreateUserAsync(Auth0User pUser)
        {
            // Make the request to Auth0
            var httpResponse = await myHttpClient.PostAsJsonAsync($"/api/v2/users", pUser, auth0JsonSerializerOptions);

            // Get the response from Auth0 if successful
            if (httpResponse.IsSuccessStatusCode)
            {
                var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                var userResponse = JsonSerializer.Deserialize<Auth0UserResponse>(jsonResponse, auth0JsonSerializerOptions);
                return userResponse;
            }
            else
            {
                // Handle the error from Auth0
                await HandleErrorAsync(httpResponse);
                throw new Exception("Error creating the user in Auth0");
            }
        }

        /// <summary>
        /// Adds a role to a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task AddRoleToUserAsync(string userId, string roleId)
        {
            // Make the request to Auth0
            var httpResponse = await myHttpClient.PostAsJsonAsync($"/api/v2/users/{userId}/roles", new { roles = new string[] { roleId } });

            // Get the response from Auth0 if successful
            if (!httpResponse.IsSuccessStatusCode)
            {
                // Handle the error from Auth0
                await HandleErrorAsync(httpResponse);
                throw new Exception("Error adding the role to the user in Auth0");
            }
        }

        /// <summary>
        /// Deletes a user from Auth0.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteUserAsync(string userId)
        {
            // Make the request to Auth0
            var httpResponse = await myHttpClient.DeleteAsync($"/api/v2/users/{userId}");

            // Get the response from Auth0 if successful
            if (!httpResponse.IsSuccessStatusCode)
            {
                // Handle the error from Auth0
                await HandleErrorAsync(httpResponse);
             
                throw new Exception("Error deleting the user in Auth0");
            }
        }


        public async Task RemoveUserRoleAsync(string userId, string roleId)
        {
            // Make the request to Auth0
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{myConnectionData.Domain}/api/v2/users/{userId}/roles"),
                Content = JsonContent.Create(new { roles = new string[] { roleId } })
            };

            var httpResponse = await myHttpClient.SendAsync(request);

            // Get the response from Auth0 if successful
            if (!httpResponse.IsSuccessStatusCode)
            {
                // Handle the error from Auth0
                await HandleErrorAsync(httpResponse);
                throw new Exception("Error removing the role from the user in Auth0");
            }
        }




        /// <summary>
        /// Handles the error from Auth0.
        /// </summary>
        /// <param name="httpResponseMessage">The http response from Auth0</param>
        /// <returns></returns>
        private async static Task HandleErrorAsync(HttpResponseMessage httpResponseMessage)
        {
            // Configure the json serializer options
            var jsonSerializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new ErrorJsonNamingPolicy(),
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
            // Get the error from the response
            Auth0Error error = await httpResponseMessage.Content.ReadFromJsonAsync<Auth0Error>(jsonSerializeOptions);


            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new Auth0BadRequestException(error.Message);
                case HttpStatusCode.Unauthorized:
                    throw new Auth0InvalidTokenException(error.Message);
                case HttpStatusCode.Forbidden:
                    throw new Auth0InsufficientScopeException(error.Message);
                case HttpStatusCode.NotFound:
                    throw new Auth0UserNotFoundException(error.Message);
                case HttpStatusCode.Conflict:
                    throw new Auth0UserAlreadyExistsException(error.Message);
                case HttpStatusCode.TooManyRequests:
                    throw new Auth0TooManyRequestsException(error.Message);
            }
        }
    }
}