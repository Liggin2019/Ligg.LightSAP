using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.DataModels;
using System.IO;
using System.Xml;

namespace Ligg.Infrastructure.Helpers
{
    public static class DataTableHelper
    {
        private static readonly string _typeFullName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        //*convert
        public static string ConvertToJson(this DataTable dt)
        {
            if (dt == null) return "";
            if (dt.Rows.Count == 0) return "";

            return JsonHelper.Serialize(dt);
        }

        public static IList<T> ConvertToList<T>(this DataTable dt)
        {
            var result = new List<T>();
            var rNo = "";
            var cName = "";
            try
            {
                //T t = (T)Activator.CreateInstance(typeof(T));
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    var t = System.Activator.CreateInstance(typeof(T));
                    PropertyInfo[] propertys = t.GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            rNo = j.ToString();
                            cName = dt.Columns[i].ColumnName;
                            // 属性与字段名称一致的进行赋值
                            if (pi.Name.Equals(dt.Columns[i].ColumnName))
                            {
                                // 数据库NULL值单独处理
                                if (dt.Rows[j][i] != DBNull.Value)
                                {
                                    var objVal = dt.Rows[j][i].ToString().ToAnyType(pi.PropertyType, '`', '~');
                                    pi.SetValue(t, objVal, null);
                                }
                                else
                                    pi.SetValue(t, null, null);
                                break;
                            }
                        }
                    }
                    result.Add((T)t);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(_typeFullName + ".ConvertToList Error: rowNo="+ rNo+ ", columnName=" + cName +", "+ ex.Message);
            }
        }


    }
}
