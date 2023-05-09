using System;

namespace Ligg.Uwa.Basis.SYS
{
    public  class ManagePermissionsDto
    {
        public string Id { get; set; }
        public int Type { get; set; }
        public string MasterId { get; set; } //operationId
        public string MasterName { get; set; }
        public int ActorType { get; set; }
        public int ActorTypeName { get; set; }
        public string ActorName { get; set; }
        public string ActorAccount { get; set; }
        public string CreatorAccount { get; set; }
        public DateTime CreationTime { get; set; }



    }


}
