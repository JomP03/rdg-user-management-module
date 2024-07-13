using System.ComponentModel.DataAnnotations;
using System;

namespace Services.API.Attributes
{
    /// <summary>
    /// Specifies that an IAM_ID field value has a valid format.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class IamIdParseAttribute : ValidationAttribute
    {
        /// <summary>
        /// IamIdFormatAttribute constructor.
        /// </summary>
        public IamIdParseAttribute() : base() { }

        /// <summary>
        /// IamIdFormatAttribute isValid implementation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            var iamId = value.ToString();

            Console.WriteLine("--------------------------------------------------------------------------------------------------");
            Console.WriteLine("--------------------------------------------------------------------------------------------------");
            Console.WriteLine($"{iamId}");
            Console.WriteLine("--------------------------------------------------------------------------------------------------");
            Console.WriteLine("--------------------------------------------------------------------------------------------------");

            // Split the IAM_ID into parts
            var parts = iamId.Split('|');

            // Check if the are exactly two parts
            if (parts.Length != 2)
            {
                return false;
            }

            // Second parte must be composed by only numbers
            if (!int.TryParse(parts[1], out _))
            {
                return false;
            }

            return true;
        }
    }
}
