namespace App.Services.Repositories.Shared
{
    /// <summary>
    /// Exception thrown when an entity is not found
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Exception thrown when an entity is not found
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="idName"></param>
        /// <param name="idValue"></param>
        public EntityNotFoundException(string entityName, string idName, string idValue) : base($"Entity {entityName} with {idName} {idValue} not found")
        {
        }

    }
}
