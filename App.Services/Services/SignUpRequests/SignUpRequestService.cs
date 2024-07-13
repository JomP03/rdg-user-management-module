using App.Services.Dtos;
using App.Services.Mappers;
using App.Services.Repositories;
using App.Services.Repositories.Shared;
using App.Services.Services.SignUpRequests;
using App.Services.Services.Users;
using Domain.Entities.SignUpRequests;
using Domain.Entities.Users;
using IAM.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;

namespace App.Services.Services.SignUpRequests
{
    /// <summary>
    /// 
    /// </summary>
    public class SignUpRequestService : ISignUpRequestService
    {

        private readonly IUnitOfWork myUnitOfWork;
        private readonly ISignUpRequestRepository mySignUpRequestRepository;
        private readonly IUserRepository myUserRepository;
        private readonly IUserService myUserService;

        /// <summary>
        /// Initializes a SkeletonService
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="userService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SignUpRequestService(IUnitOfWork unitOfWork, IUserService userService)
        {
            myUnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            mySignUpRequestRepository = unitOfWork.SignUpRequests;
            myUserRepository = unitOfWork.Users;
            myUserService = userService;
        }

        /// <summary>
        /// Add a Sign Up Request to database
        /// </summary>
        /// <param name="createSignUpRequestDto">The Sign Up Request to be added</param>
        public async Task<SignUpRequestResponseDto> CreateSignUpRequest(CreateSignUpRequestRequestDto createSignUpRequestDto)
        {
            // Map the skeleton dto into a skeleton domain
            var signUpRequestMapper = new SignUpRequestMapper();
            var signUpRequest = signUpRequestMapper.MapFromCreateDtoToDomain(createSignUpRequestDto);

            // Check if a user with the same email, nif or phone number already exists
            try
            {
                await myUserRepository.CheckForExistingUserAsync(signUpRequest.Email.Value, signUpRequest.Nif.Value, signUpRequest.PhoneNumber.Value);
            }
            catch (DbUpdateException ex)
            {

                if (ex.InnerException == null) { throw; }

                else if (ex.InnerException.Message.Contains("IX_SignUpRequests_Email"))
                {
                    throw new EntityAlreadyExistsException("SignUpRequest", "Email", signUpRequest.Email.Value);
                }

                else if (ex.InnerException.Message.Contains("IX_SignUpRequests_PhoneNumber"))
                {
                    throw new EntityAlreadyExistsException("SignUpRequest", "Phone Number", signUpRequest.PhoneNumber.Value);
                }
                else if (ex.InnerException.Message.Contains("IX_SignUpRequests_Nif"))
                {
                    throw new EntityAlreadyExistsException("SignUpRequest", "Nif", signUpRequest.Nif.Value);
                }
                else
                {
                    throw;
                }
            }

            await mySignUpRequestRepository.AddAsync(signUpRequest);
            await myUnitOfWork.CommitAsync();

            // Map the skeleton into a response dto and return it
            var response = signUpRequestMapper.MapFromDomainToOutDto(signUpRequest);
            return response;
        }


        /// <summary>
        /// Gets a SignUpRequest by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<SignUpRequestResponseDto?> GetByIdAsync(string id)
        {
            // Get the user from the database and if it doesn't exist return null
            var signUpRequest = await mySignUpRequestRepository.GetByIdAsync(id);
            if (signUpRequest == null)
            {
                return null;
            }

            // Map the user into a out dto
            var signUpRequestMapper = new SignUpRequestMapper();
            var response = signUpRequestMapper.MapFromDomainToOutDto(signUpRequest);

            // Return the user
            return response;
        }


        /// <summary>
        /// Gets a SignUpRequest by its iam id
        /// </summary>
        /// <param name="iamId"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<SignUpRequestResponseDto?> GetByIamIdAsync(string iamId)
        {

            Console.WriteLine($"IAM ID HEREEEE: {iamId}");

            // Get the user from the database and if it doesn't exist return null
            var signUpRequest = await mySignUpRequestRepository.GetByIamIdAsync(iamId);
            if (signUpRequest == null)
            {
                return null;
            }

            // Map the user into a out dto
            var signUpRequestMapper = new SignUpRequestMapper();
            var response = signUpRequestMapper.MapFromDomainToOutDto(signUpRequest);

            // Return the user
            return response;
        }

        /// <summary>
        /// Gets a SignUpRequest by its state
        /// </summary>
        /// <param name="state"></param>
        /// <returns> Retrieves SignUpRequests by state</returns>
        public async Task<RegistrationRequestResponseList> GetByStateAsync(string state)
        {
            // Transform the state string into a SignUpRequestStatus enum
            var signUpRequestStatus = (SignUpRequestStatus)Enum.Parse(typeof(SignUpRequestStatus), state, true);

            // Get the SignUpRequests from the database
            var signUpRequests = await mySignUpRequestRepository.GetByStateAsync(signUpRequestStatus);


            var signUpRequestMapper = new SignUpRequestMapper();

            // Create the response
            var response = new RegistrationRequestResponseList
            {
                RegistrationRequests = signUpRequests.Select(s => signUpRequestMapper.MapFromDomainToOutDto(s))
            };

            // Return the SignUpRequests
            return response;
        }

        
        /// <summary>
        /// Accepts or Rejects a SignUpRequest
        /// </summary>
        /// <param name="id"></param>
        /// <param name="signUpRequestActionDto"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<SignUpRequestResponseDto> AcceptOrRejectSignUpRequest(string id, SignUpRequestActionDto signUpRequestActionDto)
        {
            // Get the SignUpRequest from the database
            var signUpRequest = await mySignUpRequestRepository.GetByIdAsync(id);
            if (signUpRequest == null)
            {
                throw new EntityNotFoundException("SignUpRequest", "id", id);
            }

            // Get the actioner from the database
            var actioner = await myUserRepository.GetByIamIdAsync(signUpRequestActionDto.IamId);
            if (actioner == null)
            {   
                throw new EntityNotFoundException("User", "iamId", signUpRequestActionDto.IamId);
            }

            var signUpRequestMapper = new SignUpRequestMapper();

            if (signUpRequestActionDto.Action)
            {

                // Add the user to the database
                await myUserService.AddEndUserAsync(signUpRequest);

                signUpRequest.Approve(actioner, signUpRequestActionDto.Comment);

            } else
            {
                signUpRequest.Reject(actioner, signUpRequestActionDto.Comment);
            }
        
             // Update the SignUpRequest in the database
             await mySignUpRequestRepository.UpdateAsync(signUpRequest);
             await myUnitOfWork.CommitAsync();
        
           
            return signUpRequestMapper.MapFromDomainToOutDto(signUpRequest);
                      
        }

        /// <summary>
        /// Retrieves the state of a signUpRequest by its identification
        /// </summary>
        /// <param name="iamId"></param>
        /// <returns></returns>
        public async Task<StateOutDto> GetSignUpRequestState(string iamId)
        {
            // Get the SignUpRequest from the database
            var signUpRequest = await mySignUpRequestRepository.GetAllByIamIdAsync(iamId);
            if (signUpRequest == null)
            {
                throw new EntityNotFoundException("SignUpRequest", "iamId", iamId);
            }

            // Check if there is an Approved SignUpRequest
            if (signUpRequest.Any(s => s.Status == SignUpRequestStatus.Approved))
            {
                return new StateOutDto { State = "Approved" };
            }
            else if (signUpRequest.Any(s => s.Status == SignUpRequestStatus.Requested))
            {
                return new StateOutDto { State = "Requested" };
            }
             return new StateOutDto { State = "Rejected" };
       

        }
    }
}
