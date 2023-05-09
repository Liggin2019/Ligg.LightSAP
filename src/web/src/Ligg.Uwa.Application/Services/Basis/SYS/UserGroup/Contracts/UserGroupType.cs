using System.ComponentModel;

namespace Ligg.Uwa.Basis.SYS
{
    //*should in line with  ActorType
    public enum UserGroupType
    {
        [Description("角色")]
        Role =10,

        [Description("权限组")]
        AuthorizationGroup = 11,

        [Description("通讯组")]
        CommunicationGroup =12,

    }
}
