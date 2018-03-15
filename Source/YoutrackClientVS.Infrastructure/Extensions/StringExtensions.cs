using System;

namespace YouTrackClientVS.Infrastructure.Extensions
{
    public static class StringExtensions
    {
      

        public static bool Contains(this string source, string subString, StringComparison stringComparison)
        {
            return source.IndexOf(subString, stringComparison) >= 0;
        }
    }
}
