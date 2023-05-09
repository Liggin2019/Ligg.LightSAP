using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Helpers;
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Data;


namespace Ligg.Infrastructure.Utility.DataParser
{
    public static class TextDataFileHelper
    {
        private static readonly string _typeFullName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        internal static string[] Postfixes = new string[] { ".lstr", ".larr", ".ldct", ".ltbl", ".ljsn", ".elstr", ".elarr", ".eldct", ".eltbl", ".eljsn" };
        public static bool IsFileExisting(string filePath)
        {
            if (filePath.IsNullOrEmpty()) return false;
            var postfix = FileHelper.GetFileDetailByOption(filePath, FilePathComposition.Postfix).ToLower();
            if (postfix.IsNullOrEmpty()) return false;
            if (Postfixes.Any(x => x.Equals(postfix)))
            {
                return File.Exists(filePath);
            }
            return false;
        }

        internal static string GetFormatString(string filePath)
        {
            var postfix = FileHelper.GetFileDetailByOption(filePath, FilePathComposition.Postfix).ToLower();
            if (postfix == ".lstr" | postfix == ".elstr") return "Lstring";
            else if (postfix == ".lstr" | postfix == ".elstr") return "Lstring";
            else if (postfix == ".larr" | postfix == ".elarr") return "Larry";
            else if (postfix == ".ldct" | postfix == ".eldct") return "Ldict";
            else if (postfix == ".ljsn" | postfix == ".eljsn") return "Ljson";

            return "Undefined";
        }



    }

}
