using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    public static class Helper
    {
        public static string ToShortString(this string str, int limit = 100)
        {
            if (str.Length <= limit)
            {
                return str;
            }
            return str.Substring(0, limit) + "...";
        }
    }
}
