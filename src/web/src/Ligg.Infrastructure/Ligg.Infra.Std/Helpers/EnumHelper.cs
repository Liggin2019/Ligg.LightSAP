using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Reflection;


namespace Ligg.Infrastructure.Helpers
{
    public static partial class EnumHelper
    {
        private static readonly string _typeFullName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        //*list
        public static int[] GetIds<T>()
        {
            var exInfo = _typeFullName + ".GetIds Error: ";
            Type enumType = typeof(T);

            // Can't use type constraints on Value types, so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException(exInfo + "EnumHelper.T must be of type System.Enum");

            var ids = new List<int>();
            foreach (var v in Enum.GetValues(enumType))
            {
                ids.Add((int)v);
            }


            return ids.ToArray();
        }

        public static string[] GetNames<T>()
        {
            var exInfo = _typeFullName + ".GetNames Error: ";
            Type enumType = typeof(T);

            // can't use type constraints on Value types, so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException(exInfo + "EnumHelper.T must be of type System.Enum");

            var names = new List<string>();
            foreach (var v in Enum.GetNames(enumType))
            {
                names.Add((string)v);
            }
            return names.ToArray();
        }

        //*get
        public static T GetById<T>(int id, T defaultValue)
        {
            Type enumType = typeof(T);
            var vals = Enum.GetValues(enumType);
            var name = "";
            foreach (var v in vals)
            {
                if ((int)v == id)
                    name = v.ToString();
                continue;
            }

            return GetByName<T>(name, defaultValue);
        }

        public static T GetByName<T>(string name, T defaultValue)
        {
            Type enumType = typeof(T);
            var vals = Enum.GetValues(enumType) as IEnumerable<T>;
            foreach (var v in vals)
            {
                if (name == v.ToString())
                    return v;
            }
            return defaultValue;
        }

        public static T GetByName<T>(string name)
        {
            Type enumType = typeof(T);
            var vals = Enum.GetValues(enumType) as IEnumerable<T>;
            foreach (var v in vals)
            {
                if (name == v.ToString())
                    return v;
            }
            return default(T); //first one
            //return null; //non-nullable
            //var exInfo = _typeFullName + ".GetByName Error: ";
            //throw new ArgumentException(exInfo + "can't get a type");
        }
        public static string GetNameById<T>(int id)
        {
            Type enumType = typeof(T);
            string name = "";
            name = Enum.GetName(enumType, id);
            return name;
        }


        public static string GetDescription(this Enum enumObj)
        {
            FieldInfo EnumInfo = enumObj.GetType().GetField(enumObj.ToString());
            if (EnumInfo != null)
            {
                DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])EnumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (EnumAttributes.Length > 0)
                {
                    return EnumAttributes[0].Description;
                }
            }
            return enumObj.ToString();
        }

        public static bool IsOutofScope<T>(string name, bool ignoreCase)
        {
            var names = GetNames<T>();
            var names1 = names.ToLower();
            if (ignoreCase)
                return !names1.Contains(name.ToLower());
            else
                return !names1.Contains(name);
        }

        public static bool IsOutofScope<T>(int id)
        {
            var ids = GetIds<T>();
            return !(ids.Any(x=>x==id));
        }

    }
}