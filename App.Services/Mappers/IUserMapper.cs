using App.Services.Dtos;
using Domain.Entities.Users;

namespace App.Services.Mappers
{
    /// <summary>
    /// Interface for the UserMapper
    /// </summary>
    public interface IUserMapper : IMapper<User, CreateManagerUserRequestDto, UserResponse>
    {
    }
}
