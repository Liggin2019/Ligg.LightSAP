using System.ComponentModel;

namespace Ligg.Uwa.Basis.SYS
{
    public enum TenantType
    {
        [Description("基础租户")]
        Base = 0,
        [Description("CMS租户")]
        CMS = 1,
        [Description("管理系统租户")]
        MIS = 2,
    }
}
