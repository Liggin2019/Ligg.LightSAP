using System.ComponentModel.DataAnnotations.Schema;

namespace Ligg.EntityFramework.Entities
{
    [Table("scc_articles")]
    public  class Article :  Entity<long>
    {
        public string MasterId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public string Note { get; set; }
        public string Author { get; set; }
        public string Resource { get; set; }
        public bool Original { get; set; }
        public string ThumbnailPostfix { get; set; }
        public string ImagePostfix { get; set; }
        public string VideoPostfix { get; set; }

        public int? Sequence { get; set; }
        public bool Status { get; set; }



    }


}
