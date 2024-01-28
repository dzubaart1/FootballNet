using System;

namespace FootBallNet
{
    public static class StringUtils
    {
        /// <summary>
        /// Performs <see cref="string.StartsWith(string, StringComparison)"/> with <see cref="StringComparison.Ordinal"/>.
        /// </summary>
        public static bool StartsWithFast(this string content, string match)
        {
            return content.StartsWith(match, StringComparison.Ordinal);
        }
    }
}
