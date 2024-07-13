using App.Services.Dtos;
using App.Services.Mappers;
using Domain.Entities.SignUpRequests;

namespace Unit.Tests.DataSources
{
    public class SignUpRequestDataSource
    {

        public static SignUpRequest SignUpRequestA()
        {
            return CreateSignUpRequest("iam_A", "usera@isep.ipp.pt", "User A", "272826359", "933756104");
        }

        public static SignUpRequest SignUpRequestB()
        {
            return CreateSignUpRequest("iam_B", "userb@isep.ipp.pt", "User B", "204686601", "964257258");
        }

        public static SignUpRequest SignUpRequestC()
        {
            return CreateSignUpRequest("iam_C", "userc@isep.ipp.pt", "User C", "230513689", "931246584");
        }

        public static SignUpRequest SignUpRequestD()
        {
            return CreateSignUpRequest("iam_D", "usera@isep.ipp.pt", "User D", "241806682", "931258789");
        }


        public static CreateSignUpRequestRequestDto CreateSignUpRequestRequestDtoA()
        {
            return CreateCreateSignUpRequestRequestDto("iam_A", "usera@isep.ipp.pt", "User A", "272826359", "933756104");
        }

        public static CreateSignUpRequestRequestDto CreateSignUpRequestRequestDtoB()
        {
            return CreateCreateSignUpRequestRequestDto("iam_B", "userb@isep.ipp.pt", "User B", "204686601", "964257258");
        }

        public static CreateSignUpRequestRequestDto CreateSignUpRequestRequestDtoC()
        {
            return CreateCreateSignUpRequestRequestDto("iam_C", "userc@isep.ipp.pt", "User C", "230513689", "931246584");
        }

        public static CreateSignUpRequestRequestDto CreateSignUpRequestRequestDtoD()
        {
            return CreateCreateSignUpRequestRequestDto("iam_D", "usera@isep.ipp.pt", "User D", "241806682", "931258789");
        }

        public  static SignUpRequestResponseDto CreateSignUpRequestResponseDtoA()
        {
            return MapFromDomainToOutDto(SignUpRequestA());
        }


        private static CreateSignUpRequestRequestDto CreateCreateSignUpRequestRequestDto(string iamId, string email, string name, string nif, string phoneNumber)
        {
            return new CreateSignUpRequestRequestDto
            {
                IamId = iamId,
                Email = email,
                Name = name,
                Nif = nif,
                PhoneNumber = phoneNumber
            };
        }

        public static SignUpRequestActionDto CreateSignUpRequestApprove()
        {
            return new SignUpRequestActionDto { Action=true, Comment="Approved because its a student.", IamId="IamId" };
        }

        public static SignUpRequestActionDto CreateSignUpRequestReject()
        {
            return new SignUpRequestActionDto { Action = false, Comment = "Rejected because its a student.", IamId = "IamId" };
        }




        private static SignUpRequest CreateSignUpRequest(string iamId, string email, string name, string nif, string phoneNumber)
        {
            return new SignUpRequest(iamId, email, name, nif, phoneNumber);
        }

        private static SignUpRequestResponseDto MapFromDomainToOutDto(SignUpRequest domain)
        {
            var outDto = new SignUpRequestResponseDto
            {
                IamId = domain.IamId,
                Id = domain.Id.ToString(),
                Email = domain.Email.Value,
                Name = domain.Name.Value,
                Nif = domain.Nif.Value,
                PhoneNumber = domain.PhoneNumber.Value,
                Status = domain.Status.ToString(),
                CreationTime = domain.CreationTime.ToString(),
                ActionTime = domain.ActionTime.ToString(),
                ActionedBy = domain.ActionedBy != null ? domain.ActionedBy.ToString() : null
            };

            return outDto;

        }
    }
}
