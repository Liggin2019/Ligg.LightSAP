using System;

namespace Ligg.Uwa.Application.Shared
{
    public static class Consts
    {
        public const string OK = "OK";
        public const string TOKEN_KEY = "WebToken";

        //*dbType
        public const string DBTYPE_SQLSERVER = "SqlServer";   
        public const string DBTYPE_MYSQL = "MySql";
        public const string DBTYPE_SQLITE = "Sqlite";

        //*for cache
        public const string CACHEKEY_TNT_LST = "TenantList";
        public const string CACHEKEY_MNU_ITM_LST = "MenuItemList";
        public const string CACHEKEY_PMS_LST= "PermissionList";
        public const string CACHEKEY_CFG_LST = "ConfigList";
        public const string CACHEKEY_CFG_ITM_LST = "ConfigItemList";
        //public const string CACHEKEY_CAT_LST = "CategoryList";
        public const string CACHEKEY_DIR_LST = "DirectoryList";
        public const string CACHEKEY_TAG_LST = "TagList";


    }
}
