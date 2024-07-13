using System.Linq;
using System.Text.Json;

namespace IAM.Policies
{
    /// <summary>
    /// Naming policy for Auth0.
    /// </summary>
    public class Auth0JsonNamingPolicy : JsonNamingPolicy
    {
        /// <summary>
        /// Method to convert the name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string ConvertName(string name)
        {
            return string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }
}
