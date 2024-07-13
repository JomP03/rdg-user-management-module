using App.Services.Dtos;
using Domain.Entities.SignUpRequests;

namespace App.Services.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public class SignUpRequestMapper : ISignUpRequestMapper
    {
        public SignUpRequestResponseDto MapFromDomainToOutDto(SignUpRequest domain)
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

        public SignUpRequest MapFromCreateDtoToDomain(CreateSignUpRequestRequestDto inDto)
        {
            return new SignUpRequest(inDto.IamId, inDto.Email, inDto.Name, inDto.Nif, inDto.PhoneNumber);
        }
    }
}
