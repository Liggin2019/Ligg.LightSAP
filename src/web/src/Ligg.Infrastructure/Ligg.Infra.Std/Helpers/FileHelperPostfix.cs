using System;
using System.Linq;

namespace Ligg.Infrastructure.Helpers
{
    public static partial class FileHelper
    {
        //*judge
        public static bool IsTextFileByPostfix(string postfix)
        {
            var allowExt = new string[] { ".txt", ".ini", ".json", ".xml", ".csv"};
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            return allowExt.Any(c => stringComparer.Equals(c, postfix));
        }
        public static bool IsImageFileByPostfix(string postfix)
        {
            var allowExt = new string[] { ".gif", ".jpg", ".jpeg", ".bmp", ".png", ".tif" };
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            return allowExt.Any(c => stringComparer.Equals(c, postfix));
        }
        public static bool IsExecutableFileByPostfix(string postfix)
        {
            var allowExt = new string[] { ".exe", ".bat", ".cmd" };
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            return allowExt.Any(c => stringComparer.Equals(c, postfix));
        }
        public static bool IsSoundFileByPostfix(string postfix)
        {
            var allowExt = new string[] { ".MP3", ".WAV", ".WMA", ".MP2", ".Flac", ".MIDI", ".RA", ".APE", ".AAC", ".CDA", ".MOV" };
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            return allowExt.Any(c => stringComparer.Equals(c, postfix));
        }
        public static bool IsVedioFileByPostfix(string postfix)
        {
            var allowExt = new string[] { ".mp4", ".mov", ".m4v", ".avi", ".dat", ".flv", ".vob", ".mkv", ".wmv", ".asf", ".asx", ".rm", ".rmvb", ".gp" };
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            return allowExt.Any(c => stringComparer.Equals(c, postfix));
        }


    }
}





