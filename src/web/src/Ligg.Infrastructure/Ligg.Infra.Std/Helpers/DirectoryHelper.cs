﻿
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Ligg.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Ligg.Infrastructure.Helpers
{
    public static class DirectoryHelper
    {
        private static readonly string _typeFullName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        //*list
        public static string[] GetSubFiles(string dir, bool recursive = false)
        {
            if (!Directory.Exists(dir))
            {
                throw new ArgumentException("Directory: " + dir + " does not exsit!");
            }

            if (recursive) return GetRecursiveSubFilesPrivate(dir);
            else return Directory.GetFiles(dir).ToArray();
        }

        private static string[] GetRecursiveSubFilesPrivate(string dir)
        {

            var files = Directory.GetFiles(dir).ToList();
            var subDirs = Directory.GetDirectories(dir);
            foreach (var subDir in subDirs)
            {
                var files1 = GetRecursiveSubFilesPrivate(subDir);
                files.AddRange(files1.ToList());
            }

            return files.ToArray();
        }


        private static List<string> GetRecursiveSubDirectoriesPrivate(string dir)
        {

            var subDirs = Directory.GetDirectories(dir);
            var subDirList = subDirs.ToList();
            foreach (var subDir in subDirs)
            {
                var subs1 = GetRecursiveSubDirectoriesPrivate(subDir);
                subDirList.AddRange(subs1.ToList());
            }

            return subDirList;
        }

        private static string[] GetRecursiveSubFileSystemEntriesPrivate(string dir)
        {

            var entries = Directory.GetFileSystemEntries(dir).ToList();
            var subDirs = Directory.GetDirectories(dir);
            foreach (var subDir in subDirs)
            {
                var entries1 = GetRecursiveSubFileSystemEntriesPrivate(subDir);
                entries.AddRange(entries1.ToList());
            }

            return entries.ToArray();
        }

        //*get

        public static int GetSubFilesNum(string dir, bool recursive = false)
        {
            if (!Directory.Exists(dir))
            {
                throw new ArgumentException("Directory: " + dir + " does not exsit!");
            }
            if (recursive) return GetRecursiveSubFilesNum(dir);
            else return Directory.GetFiles(dir).Length;
        }
        private static int GetRecursiveSubFilesNum(string dir)
        {
            if (!Directory.Exists(dir))
            {
                throw new ArgumentException("Directory: " + dir + " does not exsit!");
            }
            var qty = Directory.GetFiles(dir).Length;
            var subDirs = Directory.GetDirectories(dir);
            foreach (var subDir in subDirs)
            {
                qty = qty + GetRecursiveSubFilesNum(subDir);
            }
            return qty;
        }


        public static string GetDirveName(string path)
        {
            if (!IsLegalAbsoluteDirectory(path)) return string.Empty;

            var array = path.Split('\\');
            var pathFist2Letters = path.Substring(0, 2);

            string dirve;
            if (pathFist2Letters == "\\\\")
            {
                dirve = "\\\\" + array[2] + "\\" + array[3];
            }
            else
            {
                dirve = array[0];
            }
            return dirve;
        }

        public static string GetParent(string dir, int level = 1)
        {
            if (level < 1) throw new ArgumentException(".GetParent error: " + "level should greater than 0; level=" + level);
            CheckPathExistence(dir);

            var rst = dir;
            for (int i = 0; i < level; i++)
            {
                if (IsTopLevelDirectory(rst))
                    throw new ArgumentException(".GetParent error: " + "Parent to the top level");
                rst = Directory.GetParent(rst).ToString();
            }
            return rst;
        }



        //*do
        public static void CreateDirectory(string dir)
        {
            if (dir.IsLegalDirectory() & !IsDirectoryExisting(dir))
            {
                CheckPathVadility(dir);
                Directory.CreateDirectory(dir);
            }
        }

        public static void DeleteDirectory(string dir)
        {
            try
            {
                if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
                {
                    foreach (string entry in Directory.GetFileSystemEntries(dir))
                    {
                        if (File.Exists(entry))
                            File.Delete(entry); //直接删除其中的文件                        
                        else
                            DeleteDirectory(entry); //递归删除子文件夹 
                    }
                    Directory.Delete(dir, true); //删除已空文件夹                 
                }
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                throw new ArgumentException(_typeFullName + ".DeleteDirectory error: " + ex.Message + ", dir=" + dir);

            }
        }


        public static string RemoveLastSlashes(string dir)
        {
            if (dir.EndsWith("\\"))
            {
                dir = dir.Substring(0, dir.Length - 1);
            }

            return dir;
        }

        //*judge
        private static bool IsTopLevelDirectory(string target)
        {
            var isSharedDir = target.GetQtyOfIncludedString("\\\\") == 1;
            if (isSharedDir)
            {
                var tmp = RemoveLastSlashes(target);
                var qty = tmp.GetQtyOfIncludedString("\\");
                if (qty == 3) return true;
            }
            else
            {
                var tmp = RemoveLastSlashes(target);
                var qty = tmp.GetQtyOfIncludedString("\\");
                if (qty == 0) return true;
            }
            return false;
        }

        public static bool IsLegalAbsoluteDirectory(string target)
        {
            if (string.IsNullOrEmpty(target)) return false;

            if (!target.IsLegalDirectory()) return false;
            if (target.Length < 2) return false;

            if (target.Length > 2)
            {
                var pathFirst3Letters = target.Substring(0, 3);
                if (pathFirst3Letters == "\\\\\\") return false;
            }

            var pathFirst2Letters = target.Substring(0, 2);
            if (pathFirst2Letters == "\\\\")
            {
                if (target.Contains(":")) return false;
            }
            else if (!target.Contains(":")) return false;

            return true;
        }


        public static bool IsDirectoryExisting(string dir)
        {
            if (dir.IsNullOrEmpty()) return false;
            return Directory.Exists(dir);
        }


        public static bool HasRecursiveSubFile(string dir)
        {
            if (!Directory.Exists(dir)) return false;

            if (Directory.GetFiles(dir).Length > 0) return true;
            var subDirs = Directory.GetDirectories(dir);
            foreach (var subDir in subDirs)
            {
                return HasRecursiveSubFile(subDir);
            }
            return false;
        }


        //*check
        public static void CheckPathExistence(string dir)
        {
            if (!Directory.Exists(dir))
            {
                throw new ArgumentException( "CheckDirectoryExistence error: " + "File does not exist! dir=" + dir);
            }
        }

        public static void CheckPathLegality(string dir)
        {
            if (!IsLegalAbsoluteDirectory(dir)) throw new ArgumentException("CheckDirectoryLegality error: " + "directory is illegal! dir=" + dir);
        }

        public static void CheckPathVadility(string dir)
        {
            CheckPathLegality(dir);
            //if (checkExistence & Directory.Exists(dir)) throw new ArgumentException("dir has existed! dir=" + dir);
            var drive = GetDirveName(dir);
            if (!Directory.Exists(drive)) throw new ArgumentException( "CheckDirectoryVadility error: " + "the drive: \"{0}\" does not exsit! ; dir= \"{1}\"".FormatWith(drive, dir));

        }




    }
}
