using System.ComponentModel.DataAnnotations.Schema;

namespace Ligg.EntityFramework.Entities
{
    [Table("scc_categories")]
    public  class Category : Tree<long>
    {
        public string ShortGuid { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public int? Sequence { get; set; }

        public bool IsPrivate { get; set; }


    }


}
