using System;
using System.IO;
using System.Text;

namespace Ligg.Infrastructure.Helpers
{
    public static class EncodingHelper
    {
        public static byte[] ConvertStringToBytesByEncoding(this string str, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            return (str == null ? null : encoding.GetBytes(str));
        }



    }
}
