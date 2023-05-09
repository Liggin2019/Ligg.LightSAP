using System.ComponentModel;

namespace Ligg.Uwa.Basis.SYS
{
    public enum PermissionType
    {
        [Description("操作权限")]
        Operate = 0,

        [Description("展示定制页面")]
        ShowCustPage = 1,

        [Description("授权配置管理者")]
        GrantAsManagerForConfig = 10,
        [Description("授权系统配置操作者")]
        GrantAsProducerForSysConfig = 11,
        [Description("授权定制配置操作者")]
        GrantAsProducerForCustConfig = 12,
        [Description("授权配置使用者")]
        GrantAsConsumerForConfigItem = 13,


    }
}

