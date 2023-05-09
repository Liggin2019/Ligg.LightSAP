
namespace Ligg.Uwa.Basis.SCC
{
    public  class AddEditArticleDto
    {
        public string Id { get; set; }
        public string MasterId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Body { get; set; }
        public string Note { get; set; }
        public string Author { get; set; }
        public string Resource { get; set; }

        public string ThumbnailPostfix { get; set; }
        public string ImagePostfix { get; set; }
        public int? Sequence { get; set; }

        public int Original { get; set; }
        public int Status { get; set; }




    }


}
