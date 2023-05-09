using System.ComponentModel;

namespace Ligg.Uwa.Application.Shared
{
    public enum ProjectStage
    {
        [Description("蓝图")]
        BluePrint = 0,
        [Description("设计")]
        Design = 1,
        [Description("实现")]
        Realization = 2,
        [Description("验证")]
        Verification = 3,
        [Description("上线")]
        GoLive = 4,
        [Description("优化")]
        Optimization = 5,
    }
}
