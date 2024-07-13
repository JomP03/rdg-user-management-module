using App.Services.Dtos;

namespace App.Services.Services.SignUpRequests
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISignUpRequestService
    {
        /// <summary>
        /// Add a Sign Up Request to database
        /// </summary>
        /// <param name="createSignUpRequestDto">dto of the sign up request to be added</param>
        Task<SignUpRequestResponseDto> CreateSignUpRequest(CreateSignUpRequestRequestDto createSignUpRequestDto);


        /// <summary>
        /// Gets a SignUpRequest by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The SignUpRequest response with the matching id</returns>
        Task<SignUpRequestResponseDto?> GetByIdAsync(string id);


        /// <summary>
        /// Gets a SignUpRequest by its iam id
        /// </summary>
        /// <param name="iamId"></param>
        /// <returns>The SignUpRequest response with the matching iam id</returns>
        Task<SignUpRequestResponseDto?> GetByIamIdAsync(string iamId);

        /// <summary>
        /// Gets a SignUpRequest by its state
        /// </summary>
        /// <param name="state"></param>
        /// <returns>The SignUpRequest response with the matching state</returns>
        Task<RegistrationRequestResponseList> GetByStateAsync(string state);

        /// <summary>
        /// Accepts or Rejects a SignUpRequest
        /// </summary>
        /// <param name="id"></param>
        /// <param name="signUpRequestActionDto"></param>
        /// <returns>The Created User</returns>
        Task<SignUpRequestResponseDto> AcceptOrRejectSignUpRequest(string id, SignUpRequestActionDto signUpRequestActionDto);


        /// <summary>
        /// Retrieves the state of a signUpRequest by its identification
        /// </summary>
        /// <param name="iamId"></param>
        /// <returns>The state of the request</returns>
        Task<StateOutDto> GetSignUpRequestState(string iamId);
    }
}
