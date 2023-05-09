using System;
using System.Data;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;
using Newtonsoft.Json;


namespace Ligg.Infrastructure.Helpers
{
    public static partial class ObjectHelper
    {
        private static readonly string _typeFullName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        //*convert
        //*judge
        public static bool IsType(Type type, string typeName)
        {
            try
            {
                if (type.ToString() == typeName) return true;
                if (type.ToString() == "System.Object") return false;
                return IsType(type.BaseType, typeName);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(_typeFullName + ".IsType Error: " + ex.Message);
            }
        }

        static internal object ConvertToAnyType(object value, Type type)
        {
            object returnValue;
            if ((value == null) || type.IsInstanceOfType(value))
            {
                return value;
            }
            var str = value as string;
            if ((str != null) && (str.Length == 0))
            {
                return null;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(type);
            bool flag = converter.CanConvertFrom(value.GetType());
            if (!flag)
            {
                converter = TypeDescriptor.GetConverter(value.GetType());
            }
            if (!flag && !converter.CanConvertTo(type))
            {
                throw new InvalidOperationException("Can't Convert Type：" + value.ToString() + "==>" + type);
            }
            try
            {
                returnValue = flag ? converter.ConvertFrom(null, null, value) : converter.ConvertTo(null, null, value, type);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Convert Type：" + value.ToString() + "==>" + type, e);
            }
            return returnValue;
        }


    }
}
