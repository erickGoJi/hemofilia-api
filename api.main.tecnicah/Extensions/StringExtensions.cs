using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace api.main.tecnicah.Extensions
{
    public static class StringExtensions
    {
        public static string UpperCaseFirstLetter(this string str) => str switch
        {
            null => throw new ArgumentNullException(nameof(str)),
            "" => throw new ArgumentException($"{nameof(str)} cannot be empty", nameof(str)),
            _ => string.Concat(str[0].ToString().ToUpper(), str.AsSpan(1))
        };

        public static string CapitalizeFirst(this string s)
        {
            bool IsNewSentense = true;
            var result = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                if (IsNewSentense && char.IsLetter(s[i]))
                {
                    result.Append(char.ToUpper(s[i]));
                    IsNewSentense = false;
                }
                else
                    result.Append(char.ToLower(s[i]));

                if (s[i] == '!' || s[i] == '?' || s[i] == '.')
                {
                    IsNewSentense = true;
                }
            }

            return result.ToString();
        }

        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string ReplaceWhitespace(this string s)
        {
            string replacement = "";
            return sWhitespace.Replace(s, replacement);
        }
        /// <summary>
        /// Function valid a string if email is a valid address
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Return a boolean if email is valid or not</returns>
        public static bool IsValidEmail(this string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

    }

}
