
using System;

namespace Ligg.Uwa.Application.Shared
{
    public class OperatorInfo
    {
        public string Id { get; set; }
        //public int Type { get; set; }

        public string Owner { get; set; }
        public string ActorId { get; set; }
        public string Token { get; set; }

        public string Account { get; set; }
        public string Name { get; set; }
        public string MasterId { get; set; }//ownner

        public bool IsMachine { get; set; }
        public bool IsRoot { get; set; }
        public bool IsAdministrator { get; set; }
        public bool IsSuper { get; set; }

        public string RoleIds { get; set; }
        public string AuthorizationGroupIds { get; set; }

        public string ThumbnailPostfix { get; set; }
        public DateTime CreationTime { get; set; }
    }


}
