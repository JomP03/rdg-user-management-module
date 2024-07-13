using App.Services.Dtos;
using Domain.Entities.SignUpRequests;


namespace App.Services.Services.Users
{
    /// <summary>
    /// Interface for the User Service
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Add a User to database
        /// </summary>
        /// <param name="createUserDto">dto of the User to be added</param>
        Task<UserResponse> AddUserAsync(CreateManagerUserRequestDto createUserDto);

        /// <summary>
        /// Removes a User from database
        /// </summary>
        /// <param name="iamId">id of the User to be removed</param>
        Task<HttpResponseMessage> RemoveUserAsync(string iamId);

        /// <summary>
        /// Gets a User by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The User response with the matching id</returns>
        Task<UserResponse?> GetByIdAsync(string id);

        /// <summary>
        /// Gets a User by its iam id
        /// </summary>
        /// <param name="iamId"></param>
        /// <returns></returns>
        Task<UserResponse?> GetByIamIdAsync(string iamId);

        /// <summary>
        /// Gets all User
        /// </summary>
        /// <returns>The list of Users</returns>
        Task<UserResponseList> GetUsersAsync();

        /// <summary>
        /// Gets all Roles
        /// </summary>
        /// <returns></returns>
        Task<RoleResponseList> GetManagerRolesAysnc();


        /// <summary>
        /// Updates a User
        /// </summary>
        /// <returns>The updated User</returns>
        Task<UserResponse> UpdateUserAsync(string id, UpdateUserRequestDto updateUserDto);


        /// <summary>
        /// Adds EndUser to database
        /// </summary>
        /// <returns>The created EndUser</returns>
        Task<UserResponse> AddEndUserAsync(SignUpRequest signUpRequest);
    }
}
