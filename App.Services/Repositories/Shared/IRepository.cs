namespace App.Services.Repositories.Shared
{
    /// <summary>
    /// Interface for every Repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {

        /// <summary>
        /// Retrieves an Entry from the database with a certain Id
        /// </summary>
        /// <param name="id">ID to find</param>
        /// <returns>entry with that Id</returns>
        Task<TEntity> GetByIdAsync(string id);

        /// <summary>
        /// Retrieves all entries from the database
        /// </summary>
        /// <returns>A list of entries from the database</returns>
        Task<List<TEntity>> GetAllAsync();

        /// <summary>
        /// Deletes an entry from the database
        /// </summary>
        /// <param name="entityToDelete">entity to delete</param>
        void Delete(TEntity entityToDelete);

        /// <summary>
        /// Adds an entry to the database
        /// </summary>
        /// <param name="entityToAdd">entity to add</param>
        Task AddAsync(TEntity entityToAdd);

        /// <summary>
        /// Updates an entry from the database
        /// </summary>
        /// <param name="entityToUpdate">entity to update</param>
        Task UpdateAsync(TEntity entityToUpdate);
    }

}
