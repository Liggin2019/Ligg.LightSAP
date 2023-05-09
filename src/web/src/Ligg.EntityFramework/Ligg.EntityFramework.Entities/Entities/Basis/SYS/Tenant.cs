using System.ComponentModel.DataAnnotations.Schema;

namespace Ligg.EntityFramework.Entities
{
    [Table("sys_tenants")]
    public  class Tenant:  Entity<long>
    {
        public int Type { get; set; }
        public string Key { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string ThumbnailPostfix { get; set; }
        public string ImagePostfix { get; set; }

        public bool HasIco { get; set; }

        public bool IsPublic { get; set; }
        public bool IsDefault { get; set; }
        public int? Sequence { get; set; }
        public bool Status { get; set; }

    }


}
