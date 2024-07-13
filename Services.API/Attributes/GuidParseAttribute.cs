using System;
using System.ComponentModel.DataAnnotations;

namespace Services.API.Attributes
{
    /// <summary>
    /// Specifies that a pId field value has valid guid value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class GuidParseAttribute : ValidationAttribute
    {
        /// <summary>
        /// GuidParseAttribute constructor.
        /// </summary>
        public GuidParseAttribute() : base() { }

        /// <summary>
        /// GuidParseAttribute isValid implementation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            var objectToString = value.ToString();
            var idParsing = Guid.TryParse(objectToString, out Guid parsedId);
            return idParsing;
        }
    }
}
