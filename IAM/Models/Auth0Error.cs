namespace IAM.Models
{
    /// <summary>
    /// Auth0 error model.
    /// </summary>
    public struct Auth0Error
    {
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public string Error { get; set; }
        public int StatusCode { get; set; }

    }
}
