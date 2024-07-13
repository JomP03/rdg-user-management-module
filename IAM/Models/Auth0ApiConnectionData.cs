namespace IAM.Models
{
    public struct Auth0ApiConnectionData
    {
        /// <summary>
        /// Auth0 tenant domain.
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// Auth0 tenant Access token.
        /// </summary>
        public string AccessToken { get; set; }
    }
}
