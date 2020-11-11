using System.Text.RegularExpressions;

namespace JacobDixon.AspNetCore.LiveSassCompile
{
    public static class StringExtensions
    {
        /// <summary>
        /// Compares the string against a given glob pattern.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="glob">The glob pattern to match, where "*" means any sequence of characters, and "?" means any single character.</param>
        /// <returns><c>true</c> if the string matches the given glob pattern; otherwise <c>false</c>.</returns>
        public static bool MatchesGlob(this string str, string glob)
        {
            return new Regex(
                "^" + Regex.Escape(glob).Replace(@"\*", ".*").Replace(@"\?", ".") + "$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
            ).IsMatch(str);
        }

        /// <summary>
        /// Checks if a string is null or empty.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns><c>true</c> if the string is null or empty; otherwise <c>false</c></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}