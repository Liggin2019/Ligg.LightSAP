
using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Extensions;
using System;
using System.Linq;


namespace Ligg.Infrastructure.Utilities.DataParserUtil
{
    public static partial class TextDataHelper
    {

        //*larray
        public static string[] GetLarrayArray(this string target, bool trim, bool clear)
        {
            if (target.IsNullOrEmpty()) return null;
            var separator = target.GetParallelSeparator();
            var arry = target.Split(separator);

            if (trim) arry = arry.Trim();
            if (clear) arry = arry.Clear();
            return arry;
        }


    }

}
