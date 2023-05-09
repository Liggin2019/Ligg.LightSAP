using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Helpers;
using Ligg.Uwa.Basis.SYS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ligg.Uwa.Application.Shared
{
    public class WebFileHelper
    {
        //*func
        public async static Task<TResult<string>> Upload(string absoluteFilePath, string allowedSuffixes, IFormFileCollection files, bool overwrite)
        {
            var rst = new TResult<string>();
            if (files == null || files.Count == 0)
            {
                rst.Message = "Please select file firstly";
                return rst;
            }
            if (files.Count > 1)
            {
                rst.Message = "Only permitted to upload 1 file";
                return rst;
            }


            var dir = Path.GetDirectoryName(absoluteFilePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            IFormFile file = files[0];
            var postFix = Path.GetExtension(file.FileName);
            if (!allowedSuffixes.IsNullOrEmpty())
            {
                var rst1 = CheckFileSuffix(postFix, allowedSuffixes);
                if (rst1.Flag == 0) return rst1;
            }

            if (!overwrite & File.Exists(absoluteFilePath))
            {
                rst.Flag = 0;
                rst.Message = "File has been existing: fileName=" + Path.GetFileName(absoluteFilePath);
                return rst;
            }

            var newFileName = Path.GetFileName(absoluteFilePath);
            try
            {
                using (FileStream fs = File.Create(absoluteFilePath))
                {
                    await file.CopyToAsync(fs);
                    //file.CopyTo(fs); 
                    fs.Flush();
                    fs.Close();
                }
                rst.Flag = 1;
                rst.Data = newFileName;
                rst.Message = "OK"; 
                //var fileInfo = FileHelper.GetFileInfo(absoluteFilePath, true);
                var arr = new String[5];
                arr[0] = (Math.Ceiling(file.Length / 1024.0)).ToString("0.00") + "KB";
                //arr[0] = fileInfo.LengthInfo;
                //arr[1] = fileInfo.ImageSizeInfo;
                //arr[2] = fileInfo.Length.ToString();
                //arr[3] = fileInfo.ImageWidth.ToString();
                //arr[4] = fileInfo.ImageHeight.ToString();
                rst.Description = GenericHelper.ConvertToJson(arr);
                //rst.Description = (Math.Ceiling(file.Length / 1024.0)).ToString("0.00") + "KB";
            }
            catch (Exception ex)
            {
                rst.Message = ex.Message;
            }
            return rst;
        }


        public static byte[] GetFileBytes(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                var bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                return bytes;

            }
        }

        //*common
        public static string GetAbsolutePath(string location, string relativePath)
        {
            if (!relativePath.IsNullOrEmpty())
            {
                var qty = relativePath.GetQtyOfIncludedString("..");
                if (qty > 0)
                {
                    location = DirectoryHelper.GetParent(location, qty);
                }
                relativePath = ClearPath(relativePath);
            }
            return Path.Combine(location, relativePath);
        }

        public static string GetLocation(WebFileUploadType webFileLocationType, string id = null)
        {
            var configHandler = new ConfigHandler();
            var locations = configHandler.GetConfigItems(((int)OrpConfigSubType.UploadLocation).ToString());
            if (webFileLocationType == WebFileUploadType.TempFile)
            {
                var obj = locations.Find(x => x.Key == WebFileUploadType.TempFile.ToString());
                if (obj != null) return obj.Value;
            }
            else if (webFileLocationType == WebFileUploadType.RootFile)
            {
                return (GlobalContext.HostingEnvironment.WebRootPath);
            }
            else if (webFileLocationType == WebFileUploadType.ContentFile)
            {
                return (GlobalContext.HostingEnvironment.ContentRootPath);
            }
            else if (webFileLocationType == WebFileUploadType.ResourceFile)
            {
                Path.Combine(GlobalContext.HostingEnvironment.ContentRootPath, "Resource");
            }
            else if (webFileLocationType == WebFileUploadType.HttpFile)
            {
                var obj = locations.Find(x => x.Key == WebFileUploadType.HttpFile.ToString());
                if (obj != null) return obj.Value;
            }
            else if (webFileLocationType == WebFileUploadType.FtpFile)
            {
                locations = configHandler.GetConfigItems(((int)OrpConfigSubType.FtpFileUploadLocation).ToString());
                var obj = locations.Find(x => x.Attribute == id);//topId
                if (obj != null) return obj.Value;
            }
            else if (webFileLocationType == WebFileUploadType.Attachment)
            {
                var obj = locations.Find(x => x.Key == WebFileUploadType.Attachment.ToString());
                if (obj != null) return obj.Value;
            }

            return null;
        }

        public static string GetDateTimeFlag(DateTime dateTime)
        {
            var yyyy = dateTime.ToString("yyyy");
            var mm = dateTime.ToString("MM");
            return yyyy + "\\" + mm;
        }


        public static TResult<string> CheckFileSuffix(string filePath, string allowedSuffixes)
        {
            var suffix = FileHelper.GetFileDetailByOption(filePath, FilePathComposition.Suffix);
            string[] allowArr = allowedSuffixes.ToLower().Split<string>(',');
            var isOk = allowArr.Where(x => x.Trim() == suffix.ToLower()).Any();
            var rst = new TResult<string>();
            rst.Flag = 1;
            if (!isOk)
            {
                rst.Flag = 0;
                rst.Message = "Uploaded file should with following allowed suffixes: " + allowedSuffixes;
            }
            return rst;
        }


        //*private
        private static string ClearPath(string filePath)
        {
            filePath = filePath.Trim();
            filePath = filePath.Trim('/');
            filePath = filePath.Replace("../", string.Empty);
            filePath = filePath.Replace("..\\", string.Empty);
            filePath = filePath.Replace("./", string.Empty);
            filePath = filePath.Replace(".\\", string.Empty);
            filePath = filePath.Replace('/', Path.DirectorySeparatorChar);
            return filePath;
        }
        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            var contentType = types[ext];
            if (string.IsNullOrEmpty(contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".mp4", "video/mp4"}
            };
        }

        private static string ConvertHttpToPath(string http)
        {
            http = http.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            return http;
        }

        private static string ConvertPathToHttp(string directory)
        {
            directory = directory.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return directory;
        }

    }

    public enum WebFileUploadType
    {
        TempFile,
        RootFile,
        ContentFile,
        ResourceFile,
        HttpFile,
        FtpFile,
        Attachment,
    }
    public enum UploadType
    {
        None = 0,
        Upload = 1,
        Update = 2,
    }
}
