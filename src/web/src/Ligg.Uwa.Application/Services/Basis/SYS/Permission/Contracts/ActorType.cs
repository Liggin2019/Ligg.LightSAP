using System.ComponentModel;

namespace Ligg.Uwa.Application.Shared
{
    //*must  in line with UserGroupType
    public enum ActorType
    {
        [Description("用户")]
        User = 0,
        [Description("Machine")]
        Machine = 1,

        [Description("角色")]
        Role = 10,
        [Description("权限组")]
        AuthorizationGroup = 11,

    }


}
