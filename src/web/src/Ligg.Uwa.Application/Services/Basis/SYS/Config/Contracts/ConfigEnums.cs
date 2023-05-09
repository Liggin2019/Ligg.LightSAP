using System.ComponentModel;

namespace Ligg.Uwa.Basis.SYS
{

    //public enum PortalType
    //{
    //    [Description("系统")]
    //    Admin = 0,
    //    [Description("定制")]
    //    Site = 1,
    //}

    public enum PageType
    {
        [Description("系统")]
        Systematic = 0,
        [Description("定制")]
        Customized = 1,
    }

    public enum ActionLogType
    {
        [Description("正常操作")]
        Normal = 0,
        [Description("非法Logon")]
        IllegalLogon = 1,
        [Description("非法Logon")]
        IllegalVisit = 2,
        [Description("非法Demo操作")]
        IllegalDemo = 3,
        [Description("操作未允许")]
        NotPermitted = 10,
        [Description("对象不允许操作")]
        ObjectNotPermitted = 11,
    }
}
