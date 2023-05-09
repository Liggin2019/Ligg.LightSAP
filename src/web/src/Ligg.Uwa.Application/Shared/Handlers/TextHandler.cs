using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Extensions;
using System.Collections.Generic;
using Ligg.Uwa.Basis.SYS;

using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Helpers;
using System.Data;
using System.Reflection;
using System;

namespace Ligg.Uwa.Application.Shared
{
    public class TextHandler
    {
        private readonly ConfigHandler _configHandler;
        public TextHandler()
        {
            _configHandler = new ConfigHandler();
        }

        //*public
        public string[] GetAtributes(ConfigItem cfgDefinitionCfgItem)
        {
            if (cfgDefinitionCfgItem == null) return null;
            var lst = new List<string>();
            if (!cfgDefinitionCfgItem.Attribute.IsNullOrEmpty()) lst.Add(cfgDefinitionCfgItem.Attribute); else lst.Add("Undefined");
            if (!cfgDefinitionCfgItem.Attribute1.IsNullOrEmpty()) lst.Add(cfgDefinitionCfgItem.Attribute1); else lst.Add("Undefined");
            if (!cfgDefinitionCfgItem.Attribute2.IsNullOrEmpty()) lst.Add(cfgDefinitionCfgItem.Attribute2); else lst.Add("Undefined");
            return lst.ToArray();
        }

        public string GetErrorMessage(string errKey, string errorCode = null, string addlMsg = null, string obj = null, string objId = null)
        {
            var errorDetail = GetErrorDetail(errKey, obj, objId, errorCode, addlMsg);
            string msg1 = errorDetail.Message;
            string obj1 = errorDetail.Object;
            string addtionalMsg = errorDetail.AdditionalMessage;
            string errorCode1 = errorDetail.Code;
            var errorMsg = msg1
                + (!obj1.IsNullOrEmpty() ? "; Object: " + obj1 : "")
                + (!errorCode.IsNullOrEmpty() ? "; ErrCode: " + errorCode1 : "")
                + (!addtionalMsg.IsNullOrEmpty() ? ("; " + addtionalMsg) : "");
            return errorMsg;
        }

        public ErrorDetail GetErrorDetail(string errKey, string obj, string objId, string errorCode = null, string addlMsg = null)
        {
            if (errKey.IsNullOrEmpty()) errKey = string.Empty;
            else if (errKey.ToLower() == "none" | errKey.ToLower() == "undefined")
            {
                errKey = string.Empty;
            }
            if (obj.IsNullOrEmpty()) obj = string.Empty;
            else if (obj.ToLower() == "none" | obj.ToLower() == "undefined")
            {
                obj = string.Empty;
            }
            if (objId.IsNullOrEmpty()) objId = string.Empty;
            else if (objId.ToLower() == "none" | objId.ToLower() == "undefined")
            {
                objId = string.Empty;
            }
            if (errorCode.IsNullOrEmpty()) errorCode = string.Empty;
            else if (errorCode.ToLower() == "none" | errorCode.ToLower() == "undefined")
            {
                errorCode = string.Empty;
            }

            var error = _configHandler.GetFirstOrDefaultConfigItem(((int)DevConfigSubType.Error).ToString());
            if (!errKey.IsNullOrEmpty())
            {
                var errors = _configHandler.GetConfigItems(((int)DevConfigSubType.Error).ToString());
                if (errors.Count > 0)
                    error = errors.Find(x => x.Key.ToLower() == errKey.ToLower());
            }
            var message = error.Name;


            obj = !obj.IsNullOrEmpty() ? obj +
                (!objId.IsNullOrEmpty() ? "= " + objId : "")
                : "";

            return new ErrorDetail(errorCode, obj, message, addlMsg);
        }

        //*test
        //public string GetEnumText(Enum enumObj)
        //{
        //    var enum1 = EnumHelper.GetDescription(enumObj);
        //    return null;
        //}

        //public string GetDictionaryJson(Type enumType, string names=null,bool exclude=false)
        //{
        //    return null;
        //}

        //public Dictionary<int, string> EnumToDictionary(Type enumType, string names = null, bool exclude = false)
        //{
        //    var dict = EnumHelper.EnumToDictionary(enumType, names, exclude);
        //    return dict;
        //}

        //public static List<KeyValueDescription> ToKeyValueDescriptionList(Type enumType)
        //{
        //    var keyValueDescriptionList = EnumHelper.EnumToKeyValueDescriptionList(enumType);
        //    return keyValueDescriptionList;
        //}

    }
}
