using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Services.Repositories.Shared;

namespace Persistence.Repositories.Shared
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected UMDatabaseContext myContext;
        internal DbSet<TEntity> myDbSet;

        public BaseRepository(UMDatabaseContext pContext)
        {
            myContext = pContext;
            myDbSet = myContext.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            // If the id is not a valid Guid, throw an exception
            if (!Guid.TryParse(id, out Guid parsedId))
            {
                throw new ArgumentException("Id is not a valid Guid");
            }

            var query = GetQueryWithNavigationProps();

            // Get the entity with the matching id
            return await query.SingleOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == parsedId);
        }

        public async Task AddAsync(TEntity entityToAdd)
        {
            await myDbSet.AddAsync(entityToAdd);
        }

        public async Task UpdateAsync(TEntity entityToUpdate)
        {
            myContext.Entry(entityToUpdate).State = EntityState.Modified;
            await myContext.SaveChangesAsync();
        }


        public async Task<List<TEntity>> GetAllAsync()
        {
            var query = GetQueryWithNavigationProps();

            return await query.ToListAsync();
        }

        public void Delete(TEntity entityToDelete)
        {
            myDbSet.Remove(entityToDelete);
        }

        private IQueryable<TEntity> GetQueryWithNavigationProps()
        {
            var navigationProperties = myContext.Model.FindEntityType(typeof(TEntity)).GetNavigations();

            IQueryable<TEntity> query = myDbSet;

            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty.Name);
            }

            return query;
        }
    }
}
