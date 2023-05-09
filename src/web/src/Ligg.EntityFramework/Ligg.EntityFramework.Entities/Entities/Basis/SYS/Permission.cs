using System.ComponentModel.DataAnnotations.Schema;

namespace Ligg.EntityFramework.Entities
{
    [Table("sys_permissions")]
    public  class Permission :  Entity<long>
    {
        public int Type { get; set; }

        public string MasterId { get; set; }//operationId or permissionObjectId

        public int ActorType { get; set; }//userGrp, user, machineGrp, machine, role
        public string ActorId { get; set; }
    }



}
