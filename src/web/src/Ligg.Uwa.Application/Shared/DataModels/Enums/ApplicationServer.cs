using System.ComponentModel;

namespace Ligg.Uwa.Application.Shared
{
    public enum ApplicationServer
    {
        [Description("基础应用")]
        MvcBasis = 0,
        [Description("IT服务管理系统")]
        ApiItsm = 1,
        [Description("企业信息系统")]
        ApiMis = 9,
    }
}
