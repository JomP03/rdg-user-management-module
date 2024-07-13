using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Services.API.Attributes
{
    /// <summary>
    /// Validates the range of a property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RangeAttribute : ValidationAttribute
    {
        private const char SEPARATOR = ',';

        /// <summary>
        /// Allowed values separated by ,
        /// </summary>
        public string AllowedValues { get; }


        /// <summary>
        /// Initializes a new instance of the RangeAttribute
        /// </summary>
        /// <param name="allowedValues">String with the allow values. Use ',' as the separator</param>
        public RangeAttribute(string allowedValues) : base()
        {
            AllowedValues = allowedValues;
        }

        /// <summary>
        ///     Checks if the value is valid
        /// </summary>
        /// <param name="pValue"></param>
        /// <returns></returns>
        public override bool IsValid(object pValue)
        {
            var values = AllowedValues.Split(SEPARATOR);
            return values.Contains(pValue.ToString());
        }
    }
}
