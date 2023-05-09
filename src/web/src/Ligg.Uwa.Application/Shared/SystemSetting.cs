

namespace Ligg.Uwa.Application.Shared
{
    public class SystemSetting
    {

        //*system
        public bool Debug { get; set; }
        public bool IsDemo { get; set; }
        public string CorsPolicy { get; set; }
        public string OnlyCorsSite { get; set; }
        public string CorsSites { get; set; }
        public string VirtualDirectory { get; set; }

        //*application
        public string ApplicationServer { get; set; }
        public int ApplicationServerId { get; set; }
        public bool SupportMultiTenants { get; set; }
        public bool SupportMultiLanguages { get; set; }
        public string DefaultCulture { get; set; }
        public string DefaultLanguage { get; set; }

        public bool ExclusiveOnline { get; set; }
        public string LogonMode { get; set; }

        //*db
        public string DbProvider { get; set; }
        public string DbConnectionString { get; set; }

        public int DbCommandTimeoutSeconds { get; set; }
        
        //慢查询记录Sql(秒),保存到文件以便分析
        public int DbSlowSqlLogTimeSeconds { get; set; }
        public string DbBackupDir { get; set; }

        //*logger
        public string LoggerProvider { get; set; }

        //*cache
        public string CacheProvider { get; set; }

        public string RedisConnectionString { get; set; }

        //*jwt
        public string JwtSecurityKey { get; set; }
        public int JwtExpiringHours { get; set; } //days
        public string JwtIssuer { get; set; } 
        public string JwtAudience { get; set; }

        //*running params
        public int GetMaxSequenceStep { get; set; }
        public int SnowFlakeWorkerId { get; set; }




    }
}