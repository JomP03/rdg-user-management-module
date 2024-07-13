namespace Domain.Shared
{
    public static class Guard
    {
        /// <summary>
        /// Check if a string only contains alphanumeric caracters
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool ContainsOnlyAlphanumerics(string input)
        {
            // Check if the string is null or empty
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            // Check if the string is only contains alphanumeric caracters
            foreach (char c in input)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if a string only contains letters and spaces
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool ContainsOnlyLettersAndSpaces(string input)
        {
            // Check if the string is null or empty
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            // Check if the string is only contains alphanumeric caracters
            foreach (char c in input)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Check if a string only contains numeric caracters
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumeric(string input)
        {
            // Check if the string is null or empty
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            // Check if the string is only contains numeric caracters
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}