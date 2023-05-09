using System.ComponentModel;

namespace Ligg.Uwa.Application.Shared
{
    public enum StatusType
    {
        [Description("禁用")]
        Disabled = 0,

        [Description("启用")]
        Enabled = 1


    }

    public enum GenderType
    {
        [Description("未知")]
        Unknown = 0,

        [Description("男")]
        Male = 1,

        [Description("女")]
        Female = 2
    }

    public enum YesOrNo
    {
        [Description("否")]
        No = 0,
        [Description("是")]
        Yes = 1,


    }

    public enum HasOrNone
    {
        [Description("无")]
        None = 0,
        [Description("有")]
        Has = 1,

    }

    public enum HowMany
    {
        [Description("无")]
        None = 0,
        [Description("任意")]
        Any = 1,
        [Description("部分")]
        Some = 2,
        [Description("所有")]
        All = 3,

    }

    public enum State
    {
        [Description("失败")]
        Failed = 0,

        [Description("成功")]
        Succeeded = 1
    }

    public enum ShowHideOrNone
    {
        [Description("无")]
        None = 0,
        [Description("显示")]
        Show = 1,
        [Description("隐藏")]
        Hide = 2
    }




}
