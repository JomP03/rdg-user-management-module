using System;
using System.ComponentModel.DataAnnotations;

namespace Services.API.Attributes
{
    /// <summary>
    ///  Specifies that a data field value has a required size.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ValidSizeStringAttribute : ValidationAttribute
    {
        private readonly int myStringSize;

        /// <summary>
        /// Valid Size String Attribute Constructor
        /// </summary>
        /// <param name="stringSize"></param>
        public ValidSizeStringAttribute(int stringSize) : base()
        {
            myStringSize = stringSize;
        }
        
        /// <summary>
        /// Checks if the string is valid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            var valueAsInt = value.ToString().Length;
            return valueAsInt == myStringSize;
        }
    }
}
