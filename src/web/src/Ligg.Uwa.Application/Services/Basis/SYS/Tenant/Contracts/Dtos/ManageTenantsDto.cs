using Ligg.EntityFramework.Entities.Helpers;
using Newtonsoft.Json;
using System;

namespace Ligg.Uwa.Basis.SYS
{
    public  class ManageTenantsDto 
    {
        public string Id { get; set; }
        public int Type { get; set; }
        public int Public { get; set; }
        public string Key { get; set; }
        public string Code { get; set; }

        public string ShortName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int HasThumbnail { get; set; }
        public int HasImage { get; set; }
        public int HasIco { get; set; }
        public int IsPublic { get; set; }
        public int? IsDefault { get; set; }
        public int Status { get; set; }
        public int? Sequence { get; set; }
        public DateTime ModificationTime { get; set; }
        public DateTime CreationTime { get; set; }



    }


}
