using System;
using System.ComponentModel.DataAnnotations;

namespace Services.API.Attributes
{
    /// <summary>
    /// Specifies that a data field value has a required minimum value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MinValueAttribute : ValidationAttribute
    {
        private readonly int myMinValue;

        /// <summary>
        /// Constructor for the MinValueAttr
        /// </summary>
        /// <param name="minValue"></param>
        public MinValueAttribute(int minValue) : base()
        {
            myMinValue = minValue;
        }

        /// <summary>
        /// Checks if the value is valid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            var valueAsInt = value as int?;
            if (valueAsInt == null)
            {
                return false;
            }
            return valueAsInt >= myMinValue;
        }

    }
}
