using System.ComponentModel;

namespace Ligg.Uwa.Basis.SYS
{
    public enum MenuItemType
    {
        [Description("目录")]
        Directory = 0,

        [Description("系统页面")]
        SystematicPage = 1,
        //[Description("Erd视图")]
        //ErdView = 11,
        [Description("Vue视图")]
        VueView = 12,

        [Description("定制页面")]
        CustomizedPage =2,
        [Description("外链")]
        OuterLink = 3
    }
}
