using System.ComponentModel.DataAnnotations.Schema;

namespace Ligg.EntityFramework.Entities
{
    [Table("sys_organizations")]
    public class Organization : Tree<long>
    {
        public string Owner { get; set; }
        public string Type { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }

        

        public string Description { get; set; }

        public int? Sequence { get; set; }

    }
}
