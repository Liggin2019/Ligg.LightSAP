using System.ComponentModel.DataAnnotations.Schema;

namespace Ligg.EntityFramework.Entities
{
    [Table("sys_configs")]
    public  class Config:  Entity<long>
    {
        public int Type { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int? Sequence { get; set; }
        public bool Status { get; set; }

    }


}
