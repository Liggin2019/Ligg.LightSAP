
namespace Ligg.Uwa.Basis.SCC
{
    public  class AddEditCategoryDto
    {
        public string Id { get; set; }

        public string ShortGuid { get; set; }

        public string ParentId { get; set; }

        public int Type { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int? Sequence { get; set; }
        public int Status { get; set; }

        //[NotMapped]
        public string ParentName { get; set; }

    }


}
