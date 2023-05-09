using System.ComponentModel;

namespace Ligg.Uwa.Basis.SYS
{
    public enum PermittedOperatorType
    {
        [Description("用户")]
        OnlyUser = 0,
        [Description("机器")]
        OnlyMachine = 1,
        [Description("用户和机器")]
        UserAndMachine = 0,
    }
}
