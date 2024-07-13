namespace App.Services.Repositories.Shared
{
    /// <summary>
    /// Exception thrown when an entity already exists
    /// </summary>
    public class EntityAlreadyExistsException : Exception
    {
        /// <summary>
        /// Exception thrown when an entity already exists
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="fieldThatShouldBeUnique"></param>
        /// <param name="fieldValue"></param>
        public EntityAlreadyExistsException(string entityName, string fieldThatShouldBeUnique, string fieldValue) : base($"{entityName} with {fieldThatShouldBeUnique} {fieldValue} already exists")
        {
        }

    }
}
