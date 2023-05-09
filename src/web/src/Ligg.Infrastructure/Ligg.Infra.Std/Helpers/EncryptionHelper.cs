using Ligg.Infrastructure.Extensions;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

//ov--
namespace Ligg.Infrastructure.Helpers
{
    public static partial class EncryptionHelper
    {
        private static string _selfKey1 = "mOtXb01/2Mp8kIOYD/hbAg==";
        private static string _selfKey2 = "wEdL50/eAJFSx+0thR2hhg==";
        public static string Key1 { private get; set; }
        public static string Key2 { private get; set; }


        private static readonly string _typeFullName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;


        //*md5
        public static string Md5EncryptToHex(string clearText, int bit = 32, string key = "")
        {
            var bytes = Md5EncryptPrivate(clearText, key);
            string rst = BytesToHexDefault(bytes);
            if (bit == 16)
            {
                return rst.Substring(8, 16).ToUpper();
            }
            else
            {
                return rst.ToUpper();
            }
        }
        private static Byte[] Md5EncryptPrivate(string clearText, string key)
        {
            var myKey= !string.IsNullOrEmpty(key) ? key : (!string.IsNullOrEmpty(Key1) ? Key1 : _selfKey1);
            MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] byteArr = md5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(clearText + myKey));

            return byteArr;
        }

    }


    //*common 
    //*private
    public static partial class EncryptionHelper
    {
        private static string HexStr = "1386666888oldage";
        private static char[] HexCharArr = HexStr.ToCharArray();

        private static string BytesToHexDefault(byte[] btArr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in btArr)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }


    }


}

