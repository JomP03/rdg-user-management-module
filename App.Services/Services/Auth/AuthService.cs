using App.Services.Repositories;
using App.Services.Repositories.Shared;
using Domain.Entities.SignUpRequests;
using Domain.Entities.Users;

namespace App.Services.Services.Auth
{
    public class AuthService : IAuthService
    {

        private readonly IUnitOfWork myUnitOfWork;
        private readonly ISignUpRequestRepository mySignUpRequestRepository;
        private readonly IUserRepository myUserRepository;

        public AuthService(IUnitOfWork unitOfWork)
        {
            myUnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            mySignUpRequestRepository = unitOfWork.SignUpRequests;
            myUserRepository = unitOfWork.Users;
        }


        public async Task<string> VerifyIfUserExists(string iamId)
        {


            // Check if there is a user with the given iam id
            var user = await myUserRepository.GetByIamIdAsync(iamId);

            // If there is a user, return the role
            if (user != null)
            {
                return user.Role.Type.ToString();
            }



            // Check if there is a sign up request with the given iam id
            var signUpRequest = await mySignUpRequestRepository.GetByIamIdAsync(iamId);             

            // If there is a SignUp Request, and its status is Requested, return the role
            if (signUpRequest != null && signUpRequest.Status == SignUpRequestStatus.Requested)
            {
                return "awaiting-approval";
            }

            if (signUpRequest != null && signUpRequest.Status == SignUpRequestStatus.Rejected)
            {
                return "rejected";
            }


            // If there is no user or sign up request, return null
            return null;
        }
    }
}
