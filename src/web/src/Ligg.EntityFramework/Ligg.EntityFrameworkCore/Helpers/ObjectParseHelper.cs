using System;
using System.Collections;
using System.Collections.Generic;

namespace Ligg.EntityFrameworkCore.Helpers
{
    public static partial class ObjectParseHelper
    {

        public static string[] ParseToStringArray<T>(object obj)
        {
            try
            {
                var list = obj as IEnumerable<T>;
                if (list != null)
                {
                    var ids=string.Join(",", list);
                    return ids.Split(',');
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }

        }

        public static long ParseToLong(object obj)
        {
            try
            {
                return long.Parse(obj.ToString());
            }
            catch
            {
                return 0L;
            }
        }


        public static int ParseToInt(object str)
        {
            try
            {
                return Convert.ToInt32(str);
            }
            catch
            {
                return 0;
            }
        }


        public static int ParseToInt(object str, int defaultValue)
        {
            if (str == null)
            {
                return defaultValue;
            }
            try
            {
                return Convert.ToInt32(str);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static short ParseToShort(object obj)
        {
            try
            {
                return short.Parse(obj.ToString());
            }
            catch
            {
                return 0;
            }
        }

        public static short ParseToShort(object str, short defaultValue)
        {
            try
            {
                return short.Parse(str.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }



        public static decimal ParseToDecimal(object str, decimal defaultValue)
        {
            try
            {
                return decimal.Parse(str.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }


        public static decimal ParseToDecimal(object str)
        {
            try
            {
                return decimal.Parse(str.ToString());
            }
            catch
            {
                return 0;
            }
        }

        public static bool ParseToBool(object str)
        {
            try
            {
                return bool.Parse(str.ToString());
            }
            catch
            {
                return false;
            }
        }


        public static bool ParseToBool(object str, bool result)
        {
            try
            {
                return bool.Parse(str.ToString());
            }
            catch
            {
                return result;
            }
        }

        public static float ParseToFloat(object str)
        {
            try
            {
                return float.Parse(str.ToString());
            }
            catch
            {
                return 0;
            }
        }

        public static float ParseToFloat(object str, float result)
        {
            try
            {
                return float.Parse(str.ToString());
            }
            catch
            {
                return result;
            }
        }




        public static string ParseToString(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return string.Empty;
                }
                else
                {
                    return obj.ToString();
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ParseToStrings<T>(object obj)
        {
            try
            {
                var list = obj as IEnumerable<T>;
                if (list != null)
                {
                    return string.Join(",", list);
                }
                else
                {
                    return obj.ToString();
                }
            }
            catch
            {
                return string.Empty;
            }

        }

        public static double ParseToDouble(object obj)
        {
            try
            {
                return double.Parse(obj.ToString());
            }
            catch
            {
                return 0;
            }
        }


        public static double ParseToDouble(object str, double defaultValue)
        {
            try
            {
                return double.Parse(str.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        #region 转换为DateTime
        /// <summary>
        /// 将string转换为DateTime，若转换失败，则返回日期最小值。不抛出异常。  
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ParseToDateTime(this string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return DateTime.MinValue;
                }
                if (str.Contains("-") || str.Contains("/"))
                {
                    return DateTime.Parse(str);
                }
                else
                {
                    int length = str.Length;
                    switch (length)
                    {
                        case 4:
                            return DateTime.ParseExact(str, "yyyy", System.Globalization.CultureInfo.CurrentCulture);
                        case 6:
                            return DateTime.ParseExact(str, "yyyyMM", System.Globalization.CultureInfo.CurrentCulture);
                        case 8:
                            return DateTime.ParseExact(str, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                        case 10:
                            return DateTime.ParseExact(str, "yyyyMMddHH", System.Globalization.CultureInfo.CurrentCulture);
                        case 12:
                            return DateTime.ParseExact(str, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);
                        case 14:
                            return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        default:
                            return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                    }
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// 将string转换为DateTime，若转换失败，则返回默认值。  
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ParseToDateTime(this string str, DateTime? defaultValue)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return defaultValue.GetValueOrDefault();
                }
                if (str.Contains("-") || str.Contains("/"))
                {
                    return DateTime.Parse(str);
                }
                else
                {
                    int length = str.Length;
                    switch (length)
                    {
                        case 4:
                            return DateTime.ParseExact(str, "yyyy", System.Globalization.CultureInfo.CurrentCulture);
                        case 6:
                            return DateTime.ParseExact(str, "yyyyMM", System.Globalization.CultureInfo.CurrentCulture);
                        case 8:
                            return DateTime.ParseExact(str, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                        case 10:
                            return DateTime.ParseExact(str, "yyyyMMddHH", System.Globalization.CultureInfo.CurrentCulture);
                        case 12:
                            return DateTime.ParseExact(str, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);
                        case 14:
                            return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        default:
                            return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                    }
                }
            }
            catch
            {
                return defaultValue.GetValueOrDefault();
            }
        }
        #endregion

        public static IEnumerable<TResult> CastSuper<TResult>(IEnumerable source)
        {
            foreach (object item in source)
            {
                yield return (TResult)Convert.ChangeType(item, typeof(TResult));
            }
        }
    }

}
