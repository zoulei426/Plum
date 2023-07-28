using System;

namespace Plum
{
    public static class UriExtensions
    {
        public static Uri ToUri(this string uriStr)
        {
            if (uriStr.IsNullOrWhiteSpace())
                return null;
            return new Uri(uriStr);
        }
    }
}