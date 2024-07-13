using App.Services.Repositories;
using App.Services.Repositories.Shared;
using Domain.Entities.SignUpRequests;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Shared;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(UMDatabaseContext context) : base(context)
        {
        }

        public async Task<User> GetByIamIdAsync(string iamId)
        {
            return await myContext.Users
                .Include(u => u.Role)  // Inclui a propriedade de navegação Role
                .FirstOrDefaultAsync(s => s.IamId == iamId);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await myContext.Users
                .Include(u => u.Role)  // Inclui a propriedade de navegação Role
                .FirstOrDefaultAsync(s => s.Email.Value == email);
        }

        public async Task CheckForExistingUserAsync(string email, string nif, string phoneNumber)
        {
            bool emailExists = await myContext.Users.AnyAsync(u => u.Email.Value == email);
            bool nifExists = await myContext.Users.AnyAsync(u => u.Nif.Value == nif);
            bool phoneNumberExists = await myContext.Users.AnyAsync(u => u.PhoneNumber.Value == phoneNumber);

            if (emailExists)
            {
                throw new EntityAlreadyExistsException("User", "Email", email);
            }

            if (nifExists)
            {
                throw new EntityAlreadyExistsException("User", "Nif", nif);
            }

            if (phoneNumberExists)
            {
                throw new EntityAlreadyExistsException("User", "Phone Number", phoneNumber);
            }
        }
    }
}
