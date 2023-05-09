namespace Ligg.Uwa.Basis.SCC
{
    public class ListCategoriesDto
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }

        public int? Sequence { get; set; }

    }
}


