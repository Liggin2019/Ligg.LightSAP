using System;

namespace Ligg.Uwa.Basis.SCC
{
    public  class ManageCategoriesDto
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Sequence { get; set; }
        public int Status { get; set; }

        public DateTime ModificationTime { get; set; }


    }


}
