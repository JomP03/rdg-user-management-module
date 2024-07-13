namespace App.Services.Dtos
{
    /// <summary>
    /// Dto for the Role response
    /// </summary>
    public struct RoleResponse
    {
        /// <summary>
        /// Role's identification.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Role's name.
        /// </summary>
        public string Name
        {
            get; set;
        }
    }

    /// <summary>
    /// List of roles responses
    /// </summary>
    public struct RoleResponseList
    {
        /// <summary>
        /// List of Roles
        /// </summary>
        public IEnumerable<RoleResponse> Roles { get; set; }
    }
}

