using Ligg.Infrastructure.Helpers;
using System;

namespace Ligg.Infrastructure.Extensions
{
    public static partial class StringExtension
    {
        //*to
        public static String ToUniqueStringByDataTime(this string target, DateTime dateTime, string separator)
        {
            if (separator.IsNullOrEmpty()) separator = "";
            if (target.IsNullOrEmpty()) target = "";
            var str = dateTime.ToString("yyMMddHHmmssfff");
            return target + separator + str;
        }

        public static Object ToAnyType(this String target, Type type, char spacingChar, char lineBreakChar)
        {
            if (String.IsNullOrEmpty(target))
                return null;
            if (type == null)
                return target;
            if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                String[] strs = target.Split(new char[] { lineBreakChar });
                Array array = Array.CreateInstance(elementType, strs.Length);
                for (int i = 0, c = strs.Length; i < c; ++i)
                {
                    var tempStr = strs[i].Replace(spacingChar.ToString(), " ");
                    array.SetValue(ObjectHelper.ConvertToAnyType(tempStr, elementType), i);
                }
                return array;
            }
            return ObjectHelper.ConvertToAnyType(target, type);
        }



    }
}