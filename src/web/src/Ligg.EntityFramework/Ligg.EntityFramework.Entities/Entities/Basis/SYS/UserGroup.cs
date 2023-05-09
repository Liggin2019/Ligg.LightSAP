using System.ComponentModel.DataAnnotations.Schema;

namespace Ligg.EntityFramework.Entities
{
    [Table("sys_usergroups")]
    public  class UserGroup :  Entity<long>
    {
        public int Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int? Sequence { get; set; }
        public bool Status { get; set; }

        [NotMapped]
        public bool Builtin { get; set; }

    }

}
