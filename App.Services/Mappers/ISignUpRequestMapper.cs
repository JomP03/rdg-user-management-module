using App.Services.Dtos;
using Domain.Entities.SignUpRequests;

namespace App.Services.Mappers
{
    /// <summary>
    /// Interface for the SignUpRequestMapper
    /// </summary>
    public interface ISignUpRequestMapper : IMapper<SignUpRequest, CreateSignUpRequestRequestDto, SignUpRequestResponseDto>
    {
    }
}
