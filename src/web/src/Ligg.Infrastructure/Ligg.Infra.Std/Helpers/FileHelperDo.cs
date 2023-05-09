using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Ligg.Infrastructure.Helpers
{
    public static partial class FileHelper
    {
        public static void CreateDirBeforeSave(string filePath)
        {
            var dir = GetFileDetailByOption(filePath, FilePathComposition.Directory);
            if (System.IO.Directory.Exists(dir)) return;
            CheckPathBeforeSaveAs(filePath);
            Directory.CreateDirectory(dir);
        }

        public static void Delete(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }



    }
}




