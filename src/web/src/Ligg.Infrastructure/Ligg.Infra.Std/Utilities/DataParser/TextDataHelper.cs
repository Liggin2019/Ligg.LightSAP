
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
        private static readonly string _typeFullName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        //` ~ 最后再用
        //取消 - 这个太常用; _ 可以考虑給_keyValueSeparators
        //取消 '<', '>','&'  remark: xml不支持:>输不进去； < &读取报错 XmlHelper.ConvertToGeneric Error: XML 文档(41, 126)中有错误。
        private static readonly char[] _parallelSeparators = new char[] { '!', '|', ',', '*', ' ' };

        private static readonly char[] _keyValueSeparators = new char[] { '=', ':' };
        private static readonly char[] _dataSeparators = new char[] { '\n' };


        //*private
        private static char GetSeparator(this string target, char[] separators)
        {
            var len = separators.Length;
            var separator = separators[len - 1];
            if (target.IsNullOrEmpty()) return separator;
            for (int i = 0; i < len; i++)
            {
                if (target.Contains(separators[i]))
                {
                    return separators[i];
                }
            }

            return separator;
        }


        private static char GetParallelSeparator(this string target)
        {
            return target.GetSeparator(_parallelSeparators);
        }

        private static string[] GetLdictKeyValuePair(string target)
        {
            var separator = target.GetKeyValueSeparator();
            var arry = target.Split(separator);
            return arry;
        }

        private static char GetKeyValueSeparator(this string target)
        {
            var separator = target.GetSeparator(_keyValueSeparators);
            return separator;
        }

        //*common
        public static bool JudgeJudgementFlag(this string target)
        {
            if (target.IsNullOrEmpty()) return false;
            if (target.ToLower() == "true") return true;
            return false;
        }


    }
}
