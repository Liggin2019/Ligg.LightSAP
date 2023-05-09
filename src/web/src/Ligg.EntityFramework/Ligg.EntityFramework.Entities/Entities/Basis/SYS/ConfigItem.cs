using System.ComponentModel.DataAnnotations.Schema;

namespace Ligg.EntityFramework.Entities
{

    [Table("sys_configitems")]
    public class ConfigItem : Entity<long>
    {
        public string MasterId { get; set; }
        public string Key { get; set; }
        public string Attribute { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Authorization { get; set; }
        public string PermissionMark { get; set; }

        public string Style { get; set; }

        public bool IsDefault { get; set; }

        public int? Sequence { get; set; }

        public bool Status { get; set; }


    }

}
