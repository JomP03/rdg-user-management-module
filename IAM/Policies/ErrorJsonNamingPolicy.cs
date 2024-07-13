using System.Text.Json;

namespace IAM.Policies
{
    /// <summary>
    /// Naming policy for errors.
    /// </summary>
    public class ErrorJsonNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return char.ToLower(name[0]) + name.Substring(1);
        }
    }
}
