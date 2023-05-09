using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;


namespace Ligg.Infrastructure.Helpers
{
    public static partial class EnumHelper
    {
        public static List<KeyValueDescription> EnumToKeyValueDescriptionList(Type enumType,string names = null, bool exclude = false)
        {
            var list = new List<KeyValueDescription>();
            Type typeDescription = typeof(DescriptionAttribute);
            FieldInfo[] fields = enumType.GetFields();
            string[] fieldArr = null;
            if (names != null)
            {
                var fieldArr1 = names.Split(' ').Trim().Clear().ToLower();
                if (fieldArr1 != null)
                {
                    if (fieldArr1.Length > 0) fieldArr = fieldArr1;
                }
            }
            foreach (FieldInfo fieldInfo in fields)
            {
                if (fieldArr != null)
                {
                    if (!exclude & !fieldArr.Contains(fieldInfo.Name.ToLower())) continue;
                    if (exclude & fieldArr.Contains(fieldInfo.Name.ToLower())) continue;
                }
                if (fieldInfo.FieldType.IsEnum)
                {
                    var keyValueDescription = new KeyValueDescription();
                    keyValueDescription.Key = ((int)enumType.InvokeMember(fieldInfo.Name, BindingFlags.GetField, null, null, null)).ToString();
                    keyValueDescription.Value = fieldInfo.Name;
                    object[] arr = fieldInfo.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        DescriptionAttribute da = (DescriptionAttribute)arr[0];
                        keyValueDescription.Description = da.Description;
                    }
                    else
                    {
                        //keyValueDescription.Description = field.Name;
                    }
                    list.Add(keyValueDescription);
                }
            }
            return list.Count == 0 ? null : list;
        }

        public static string EnumToDictionaryJson(this Type enumType, string names = null, bool exclude = false)
        {
            List<KeyValuePair<int, string>> dict = EnumToDictionary(enumType, names, exclude).ToList();
            var json = JsonConvert.SerializeObject(dict);
            return json;
        }

        public static Dictionary<int, string> EnumToDictionary(this Type enumType, string names = null, bool exclude = false)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            Type typeDescription = typeof(DescriptionAttribute);
            FieldInfo[] fieldInfos = enumType.GetFields();
            string[] fieldArr = null;
            if (names != null)
            {
                var fieldArr1 = names.Split(' ').Trim().Clear().ToLower();
                if (fieldArr1 != null)
                {
                    if (fieldArr1.Length > 0) fieldArr = fieldArr1;
                }
            }

            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldArr != null)
                {
                    if (!exclude & !fieldArr.Contains(fieldInfo.Name.ToLower())) continue;
                    if (exclude & fieldArr.Contains(fieldInfo.Name.ToLower())) continue;
                }

                if (fieldInfo.FieldType.IsEnum)
                {
                    string desc = string.Empty;
                    var val = ((int)enumType.InvokeMember(fieldInfo.Name, BindingFlags.GetField, null, null, null));

                    object[] arr = fieldInfo.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        DescriptionAttribute da = (DescriptionAttribute)arr[0];
                        desc = da.Description;
                    }
                    else
                    {
                        desc = fieldInfo.Name;
                    }

                    dictionary.Add(val, desc);
                }
            }
            return dictionary;
        }





    }
}