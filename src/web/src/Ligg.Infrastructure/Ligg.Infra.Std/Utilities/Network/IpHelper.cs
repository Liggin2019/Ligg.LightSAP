using Ligg.Infrastructure.Helpers;
using Ligg.Infrastructure.Utilities.LogUtil;
using Ligg.Infrastructure.Utilities.WebUtil;
using System;
using System.Net;
using System.Net.Sockets;

namespace Ligg.Infrastructure.Utilities.NetworkUtil
{
    public class IpHelper
    {
        //*get
        public static string GetIpLocation(string ipAddress)
        {
            string ipLocation = "Local LAN";
            try
            {
                if (!IsLanIp(ipAddress))
                {
                    ipLocation = GetIpLocationFromTaoBao(ipAddress);
                    if (string.IsNullOrEmpty(ipLocation))
                    {
                        ipLocation = GetIpLocationFromPCOnline(ipAddress);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return ipLocation;
        }
        public static string GetLanIp()
        {
            try
            {
                foreach (var hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return hostAddress.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return string.Empty;
        }


        //*judge
        public static bool IsLanIp(string ipAddress)
        {
            bool isLanIp = false;
            long ipNum = ConvertIpToLong(ipAddress);
            /**
                私有IP：A类 10.0.0.0-10.255.255.255
                            B类 172.16.0.0-172.31.255.255
                            C类 192.168.0.0-192.168.255.255
                当然，还有127这个网段是环回地址 
           **/
            long aBegin = ConvertIpToLong("10.0.0.0");
            long aEnd = ConvertIpToLong("10.255.255.255");
            long bBegin = ConvertIpToLong("172.16.0.0");
            long bEnd = ConvertIpToLong("172.31.255.255");
            long cBegin = ConvertIpToLong("192.168.0.0");
            long cEnd = ConvertIpToLong("192.168.255.255");
            isLanIp = IsInLan(ipNum, aBegin, aEnd) || IsInLan(ipNum, bBegin, bEnd) || IsInLan(ipNum, cBegin, cEnd) || ipAddress.Equals("127.0.0.1");
            return isLanIp;
        }


        //*private
        private static string GetIpLocationFromTaoBao(string ipAddress)
        {
            string url = "http://ip.taobao.com/service/getIpInfo2.php";
            string postData = string.Format("ip={0}", ipAddress);
            string result = HttpHelper.HttpPost(url, postData);
            string ipLocation = string.Empty;
            if (!string.IsNullOrEmpty(result))
            {
                var json = JsonHelper.ConvertToJObject(result);
                var jsonData = json["data"];
                if (jsonData != null)
                {
                    ipLocation = jsonData["region"] + " " + jsonData["city"];
                    ipLocation = ipLocation.Trim();
                }
            }
            return ipLocation;
        }

        private static string GetIpLocationFromPCOnline(string ipAddress)
        {
            HttpResult httpResult = new HttpHelper().GetHtml(new HttpItem
            {
                URL = "http://whois.pconline.com.cn/ip.jsp?ip=" + ipAddress,
                ContentType = "text/html; charset=gb2312"
            });

            string ipLocation = string.Empty;
            if (!string.IsNullOrEmpty(httpResult.Html))
            {
                var resultArr = httpResult.Html.Split(' ');
                ipLocation = resultArr[0].Replace("省", "  ").Replace("市", "");
                ipLocation = ipLocation.Trim();
            }
            return ipLocation;
        }

        private static bool IsInLan(long userIp, long begin, long end)
        {
            return (userIp >= begin) && (userIp <= end);
        }

        private static long ConvertIpToLong(string ipAddress)
        {
            string[] ip = ipAddress.Split('.');
            long a = int.Parse(ip[0]);
            long b = int.Parse(ip[1]);
            long c = int.Parse(ip[2]);
            long d = int.Parse(ip[3]);

            long ipNum = a * 256 * 256 * 256 + b * 256 * 256 + c * 256 + d;
            return ipNum;
        }
    }
}
