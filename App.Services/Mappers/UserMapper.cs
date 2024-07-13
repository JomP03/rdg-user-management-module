using App.Services.Dtos;
using Domain.Entities.Users;
using Domain.Entities.Users.Role;

namespace App.Services.Mappers
{
    /// <summary>
    /// Mapper for the User
    /// </summary>
    public class UserMapper : IUserMapper
    {
        /// <summary>
        /// Maps a User to a UserResponse
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public UserResponse MapFromDomainToOutDto(User domain)
        {
            var outDto = new UserResponse
            {
                Id = domain.Id.ToString(),
                Name = domain.Name.Value,
                Email = domain.Email.Value,
                PhoneNumber = domain.PhoneNumber.Value,
                RoleName = domain.Role.Type.ToString(),
                IamId = domain.IamId,
            };

            if (domain.Role.Type == RoleType.ENDUSER)
            {
                outDto.Nif = domain.Nif.Value;
            }

            return outDto;

        }

        /// <summary>
        /// Maps a CreateUserRequestDto to a User
        /// </summary>
        /// <param name="createManagerUserRequestDto"></param>
        /// <returns></returns>
        public User MapFromCreateDtoToDomain(CreateManagerUserRequestDto createManagerUserRequestDto)
        {
            // Create the user and return it
            return new User(createManagerUserRequestDto.Email,
                createManagerUserRequestDto.Name,
                createManagerUserRequestDto.PhoneNumber,
                createManagerUserRequestDto.Role,
                null,
                createManagerUserRequestDto.Password
                );
        }
    }
}
