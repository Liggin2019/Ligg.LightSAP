using System;
using System.Linq;

namespace Ligg.Infrastructure.Extensions
{
    public static partial class StringExtension
    {
        public static int GetQtyOfIncludedChar(this string target, char incChar)
        {
            int count = 0;
            for (int i = 0; i < target.Length; i++)
            {
                if (target[i] == incChar)
                {
                    count++;
                }
            }
            return count;
        }

        public static int GetQtyOfIncludedString(this string target, string incStr)
        {
            var schStr = incStr;
            if (incStr == null) return 0;
            if (string.IsNullOrEmpty(target)) return 0;
            int schStrLen = schStr.Length;
            var targetLen = target.Length;
            int index = 0;
            int pos = 0;
            int count = 0;
            do
            {
                index = target.IndexOf(schStr, pos);
                if (index != -1)
                {
                    count++;
                }
                pos = schStrLen + index;
            } while (index != -1 && pos + schStrLen < targetLen + 1);
            return count;
        }


        public static string GetSubString(this string value, int length, int startIndex=0)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            if (value.Length> length+ startIndex)
            {
                value = value.Substring(startIndex, length);
            }
            else
            {
                value = value.Substring(startIndex, value.Length- startIndex);
            }

            return value;
        }


    }
}