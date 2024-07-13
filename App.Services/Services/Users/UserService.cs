using App.Services.Dtos;
using App.Services.Mappers;
using App.Services.Repositories;
using App.Services.Repositories.Shared;
using Domain.Entities.SignUpRequests;
using Domain.Entities.Users;
using Domain.Entities.Users.Role;
using IAM.Factories;
using IAM.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Services.Users
{
    /// <summary>
    /// Service for the Users
    /// </summary>
    public class UserService : IUserService
    {

        private readonly IUnitOfWork myUnitOfWork;
        private readonly IUserRepository myUsersRepository;
        private readonly IRoleRepository myRolesRepository;
        private readonly ISignUpRequestRepository mySignupRequestsRepository;
        private readonly IUserGatewayFactory myUserGatewayFactory;
        private readonly IUserMapper userMapper;
        private readonly string auth0Domain;
        private readonly string auth0Token;
        private readonly string connectionDatabaseName;

        /// <summary>
        /// Initializes a UserService
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="userGatewayFactory"></param>
        /// <param name="userMapper"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UserService(IUnitOfWork unitOfWork, IUserGatewayFactory userGatewayFactory, IUserMapper userMapper)
        {
            myUnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            myUserGatewayFactory = userGatewayFactory ?? throw new ArgumentNullException(nameof(userGatewayFactory));
            myUsersRepository = unitOfWork.Users;
            myRolesRepository = unitOfWork.Roles;
            mySignupRequestsRepository = unitOfWork.SignUpRequests;
            auth0Domain = Environment.GetEnvironmentVariable("AUTH0_DOMAIN") ?? throw new ArgumentNullException(nameof(auth0Domain));
            // In a real production scenario, this token would be retrieved by a service that would be responsible for getting a secure token from Auth0
            auth0Token = Environment.GetEnvironmentVariable("AUTH0_TOKEN") ?? throw new ArgumentNullException(nameof(auth0Token));
            connectionDatabaseName = Environment.GetEnvironmentVariable("AUTH0_CONNECTION_DATABASE_NAME") ?? throw new ArgumentNullException(nameof(connectionDatabaseName));
            this.userMapper = userMapper;
        }

        /// <summary>
        /// Converts a User into an Auth0User to create it in Auth0
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private Auth0User ConvertIntoAuth0User(User user)
        {
            return new Auth0User
            {
                Email = user.Email.Value,
                Connection = connectionDatabaseName,
                Password = user.Password.Value,
                VerifyEmail = false
            };
        }

        /// <summary>
        /// Add a User to database
        /// </summary>
        /// <param name="createUserDto">The user to be added</param>
        public async Task<UserResponse> AddUserAsync(CreateManagerUserRequestDto createUserDto)
        {
            // Get the role from the database
            var role = await myRolesRepository.GetByIdAsync(createUserDto.RoleId) ?? throw new EntityNotFoundException("Role", "id", createUserDto.RoleId);
            createUserDto.Role = role;

            // Map the user dto into a user domain
            var user = userMapper.MapFromCreateDtoToDomain(createUserDto);

            // Convert the user into an Auth0User
            var auth0User = ConvertIntoAuth0User(user);

            // Create the user in Auth0
            var userGateway = myUserGatewayFactory.Create(auth0Domain, auth0Token);
            var auth0UserResponse = await userGateway.CreateUserAsync(auth0User);

            // Get the user's iam id and add it to the user
            var userIamId = auth0UserResponse.UserId;
            user.AddIamId(userIamId);

            // Add the role to the user
            await userGateway.AddRoleToUserAsync(userIamId, role.IamId);

            // Add the user to the database
            try
            {
                await myUsersRepository.AddAsync(user);
                await myUnitOfWork.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                // Delete the user from Auth0
                await userGateway.DeleteUserAsync(userIamId);

                if (ex.InnerException == null) { throw; }

                if (ex.InnerException.Message.Contains("IX_Users_PhoneNumber"))
                {
                    throw new EntityAlreadyExistsException("User", "Phone Number", user.PhoneNumber.Value);
                } else if (ex.InnerException.Message.Contains("IX_Users_Email"))
                {
                    throw new EntityAlreadyExistsException("User", "Email", user.Email.Value);
                } else
                {
                    throw;
                }
            }


            // Map the user into a response dto and return it
            var response = userMapper.MapFromDomainToOutDto(user);
            return response;
        }

        /// <summary>
        /// Removes a User from database
        /// </summary>
        /// <param name="iamId">The user's iam id</param>
        public async Task<HttpResponseMessage> RemoveUserAsync(string iamId)
        {
            // Get the user from the database
            var user = await myUsersRepository.GetByIamIdAsync(iamId);
            if (user == null)
            {
                throw new EntityNotFoundException("User", "id", iamId);
            }

            // Delete the user from Auth0
            var userGateway = myUserGatewayFactory.Create(auth0Domain, auth0Token);
            await userGateway.DeleteUserAsync(iamId);

            try
            {
                // Delete the user from the database
                myUsersRepository.Delete(user);
                await myUnitOfWork.CommitAsync();

                // Delete the signup request from the database
                var signupRequest = await mySignupRequestsRepository.GetByIamIdAsync(iamId);
                if (signupRequest != null)
                {
                    mySignupRequestsRepository.Delete(signupRequest);
                    await myUnitOfWork.CommitAsync();
                }

            } catch (DbUpdateException ex)
            {
                // Create the user in Auth0 again
                var auth0User = ConvertIntoAuth0User(user);
                await userGateway.CreateUserAsync(auth0User);
                await myUnitOfWork.CommitAsync();

                if (ex.InnerException == null) { throw; }
            }

            // Return status code 200
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets a User by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<UserResponse?> GetByIdAsync(string id)
        {
            // Get the user from the database and if it doesn't exist return null
            var user = await myUsersRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            // Map the user into a out dto
            var response = userMapper.MapFromDomainToOutDto(user);

            // Return the user
            return response;
        }

        /// <summary>
        /// Gets a User by its IamId
        /// </summary>
        /// <param name="iamId"></param>
        /// <returns></returns>
        public async Task<UserResponse?> GetByIamIdAsync(string iamId)
        {
            var user = await myUsersRepository.GetByIamIdAsync(iamId);
            if(user == null)
            {
                return null;
            }
            
            var response = userMapper.MapFromDomainToOutDto(user);

            return response;
        }

        /// <summary>
        /// Gets all the users 
        /// </summary>
        /// <returns></returns>
        public async Task<UserResponseList> GetUsersAsync()
        {
            var users = await myUsersRepository.GetAllAsync();


            // Create the response
            var response = new UserResponseList
            {
                Users = users.Select(s => userMapper.MapFromDomainToOutDto(s))
            };

            return response;
        }

        /// <summary>
        /// Gets all Roles
        /// </summary>
        /// <returns></returns>
        public async Task<RoleResponseList> GetManagerRolesAysnc()
        {
            var roles = await myRolesRepository.GetManagerRolesAsync();

            // Create the response
            var response = new RoleResponseList
            {
                Roles = roles.Select(s => new RoleResponse
                {
                    Id = s.Id.ToString(),
                    Name = s.Type.ToString()
                })
            };

            return response;
        }

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="id">User's id</param>
        /// <param name="updateUserDto">User's update dto</param>
        /// <returns></returns>
        public async Task<UserResponse> UpdateUserAsync(string id, UpdateUserRequestDto updateUserDto)
        {

            // Get the user from the database
            var existingUser = await myUsersRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                throw new EntityNotFoundException("User", "id", id);
            }

            // Update the user details based on the fields in the DTO
            if (!string.IsNullOrEmpty(updateUserDto.Name))
            {
                existingUser.UpdateName(updateUserDto.Name);
            }

            if (!string.IsNullOrEmpty(updateUserDto.PhoneNumber))
            {
                existingUser.UpdatePhoneNumber(updateUserDto.PhoneNumber);
            }

            if (!string.IsNullOrEmpty(updateUserDto.Nif))
            {
                existingUser.UpdateNif(updateUserDto.Nif);
            }


            try
            {
                // Save the updated user to the database
                await myUsersRepository.UpdateAsync(existingUser);
                await myUnitOfWork.CommitAsync();
            }
            catch (DbUpdateException ex)
            {

                if (ex.InnerException == null) { throw; }

                if (ex.InnerException.Message.Contains("IX_Users_PhoneNumber"))
                {
                    throw new EntityAlreadyExistsException("User", "Phone Number", existingUser.PhoneNumber.Value);
                }
                else if (ex.InnerException.Message.Contains("IX_Users_Email"))
                {
                    throw new EntityAlreadyExistsException("User", "Email", existingUser.Email.Value);
                }
                else if(ex.InnerException.Message.Contains("IX_Users_Nif"))
                {
                    throw new EntityAlreadyExistsException("User", "Nif", existingUser.Nif.Value);
                }
                else
                {
                    throw;
                }
            }


            // Map the updated user into a response dto and return it
            var response = userMapper.MapFromDomainToOutDto(existingUser);
            return response;
        }


        /// <summary>
        /// Adds EndUser to database
        /// </summary>
        /// <param name="signUpRequest"></param>
        /// <returns>The created user</returns>
        public async Task<UserResponse> AddEndUserAsync(SignUpRequest signUpRequest)
        {         
            // EndUser role
            var role = await myRolesRepository.GetEndUserRoleAsync();
   
            // Create the user in Auth0
            var userGateway = myUserGatewayFactory.Create(auth0Domain, auth0Token);
           
            var user = new User(signUpRequest.Email.Value, signUpRequest.Name.Value, signUpRequest.PhoneNumber.Value, role, signUpRequest.Nif.Value);
            user.AddIamId(signUpRequest.IamId);

            // Add the role to the user
            await userGateway.AddRoleToUserAsync(signUpRequest.IamId, role.IamId);

            // Add the user to the database
            try
            {
                await myUsersRepository.AddAsync(user);
                await myUnitOfWork.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                await userGateway.RemoveUserRoleAsync(signUpRequest.IamId, role.IamId);

                if (ex.InnerException == null) { throw; }

                if (ex.InnerException.Message.Contains("IX_Users_PhoneNumber"))
                {
                    throw new EntityAlreadyExistsException("User", "Phone Number", user.PhoneNumber.Value);
                }
                else if (ex.InnerException.Message.Contains("IX_Users_Email"))
                {
                    throw new EntityAlreadyExistsException("User", "Email", user.Email.Value);
                }
                else if (ex.InnerException.Message.Contains("IX_Users_Nif"))
                {
                    throw new EntityAlreadyExistsException("User", "Nif", user.Nif.Value);
                }
                else
                {
                    throw;
                }
            }



            // Map the user into a response dto and return it
            var response = userMapper.MapFromDomainToOutDto(user);
            return response;
        }
    }
}
