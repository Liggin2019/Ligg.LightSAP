using System.ComponentModel;

namespace Ligg.Uwa.Basis.SYS
{
    public enum UserContainerType
    {
        [Description("角色")]
        Role = 0,
        [Description("用户组")]
        UserGroup = 1,
        [Description("通讯组")]
        CommGroup= 2,
        //[Description("职位")]
        //Position = 3,
    }
}
