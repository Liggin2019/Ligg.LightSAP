using System.ComponentModel;

namespace Ligg.Uwa.Application.Shared
{
    public enum PolicyType
    {
        [Description("未定义")]
        Undefined = -1,//LetBe Disregard
        [Description("准许")]
        Allow = 0,
        [Description("禁止")]
        Disallow = 1,

    }
    public enum PolicyOption
    {
        [Description("无")]
        None = 0,
        [Description("所有")]
        Any = 1,
        [Description("部分")]
        Some = 2,
        [Description("除此之外")]
        AllBut = 3,

    }
}
