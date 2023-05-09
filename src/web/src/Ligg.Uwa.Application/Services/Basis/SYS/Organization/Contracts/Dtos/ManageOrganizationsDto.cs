
using System;

namespace Ligg.Uwa.Basis.SYS
{
    public class ManageOrganizationsDto
    {

        public string Id { get; set; }

        public string ParentId { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public int? Sequence { get; set; }

        public int Status { get; set; }
        public DateTime ModificationTime { get; set; }

        //[NotMapped]
        public string OwnerName { get; set; }



    }


}
