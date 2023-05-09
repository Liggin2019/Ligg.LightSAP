using System.ComponentModel;

namespace Ligg.Uwa.Application.Shared
{
    public enum Authorization
    {
        [Description("需要授权")]
        TobePermitted =0,
        [Description("匿名访问")]
        Anonymous = 1,
        [Description("仅限用户")]
        AnyUser =2,
        [Description("仅限机器")]
        AnyMachine = 3,
        [Description("用户或机器")]
        AnyOne =4,

    }
}
