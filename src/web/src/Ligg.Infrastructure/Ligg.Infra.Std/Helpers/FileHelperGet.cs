using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ligg.Infrastructure.Helpers
{
    public static partial class FileHelper
    {
        private static readonly string _typeFullName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        //*list
        public static string GetFileDetailByOption(string path, FilePathComposition returnOpt)
        {

            var dir = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);
            var fileTitle = Path.GetFileNameWithoutExtension(path);
            var postfix = Path.GetExtension(path);

            if (returnOpt == FilePathComposition.FilePathTitle)
            {
                return Path.Combine(dir, fileTitle);
            }
            else if (returnOpt == FilePathComposition.Directory) return dir;
            else if (returnOpt == FilePathComposition.Folder)
            {
                var arr = dir.Split('\\');
                return arr[arr.Length - 1];
            }
            else if (returnOpt == FilePathComposition.FileName) return fileName; //file name
            else if (returnOpt == FilePathComposition.FileTitle)
            {
                return fileTitle;
            }
            else if (returnOpt == FilePathComposition.Postfix)
            {
                return postfix;
            }
            else if (returnOpt == FilePathComposition.Suffix)
            {
                var surfix = "";
                if (!postfix.IsNullOrEmpty()) surfix = postfix.Substring(1, postfix.Length - 1);
                return surfix;
            }
            return string.Empty;
        }


    }
}





