
using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Ligg.Infrastructure.Utilities.DataParserUtil
{
    public static partial class TextDataHelper
    {
        public static Dictionary<string, string> ConvertLdictToDictionary(this string target, bool trim, bool clear, bool clearReturnChar)
        {
            if (target.IsNullOrEmpty()) return null;
            var arry = target.GetLdictArray(false, clearReturnChar);
            var dict = new Dictionary<string, string>();
            foreach (var str in arry)
            {
                var arr1 = GetLdictKeyValuePair(str);
                if (arr1.Length < 2) continue;
                if (arr1[0].IsNullOrEmptyOrWhiteSpace()) continue;
                if (clear) if (arr1[1].IsNullOrEmpty()) continue;
                if (!dict.ContainsKey(arr1[0].Trim()))
                    dict.Add(arr1[0].Trim(), trim ? arr1[1].Trim() : arr1[1]);
            }
            return dict;
        }

        public static string GetLdictValue(this Dictionary<string, string> dict, string key)
        {
            if (dict == null) return string.Empty;
            if (key.IsNullOrEmpty()) return string.Empty;
            if (!dict.ContainsKey(key)) return string.Empty;
            return dict[key];

        }

        public static string GetLdictValue(this string target, string key, bool trim, bool clear)
        {
            if (string.IsNullOrEmpty(target))
                return string.Empty;
            var dict = ConvertLdictToDictionary(target, trim, clear, true);
            if (dict != null) return dict.GetLdictValue(key);
            return string.Empty;
        }


        //*private
        private static string[] GetLdictArray(this string target, bool trim, bool clearReturnChar)
        {
            if (target.IsNullOrEmpty()) return null;
            if (clearReturnChar)
                target = target.Replace("\n", "");
            var strArry = target.GetLarrayArray(trim, true);
            return strArry;
        }


    }
}
