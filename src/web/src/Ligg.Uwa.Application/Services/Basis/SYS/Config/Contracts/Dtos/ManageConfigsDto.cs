using Ligg.EntityFramework.Entities.Helpers;
using Newtonsoft.Json;
using System;

namespace Ligg.Uwa.Basis.SYS
{
    public  class ManageConfigsDto 
    {
        public string Id { get; set; }
        public int Type { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string OwnerAccount { get; set; }
        public int ManagerNum { get; set; }
        public int ProducerNum { get; set; }

        public int Status { get; set; }
        //public int PutDataToFronEnd { get; set; }

        public int? Sequence { get; set; }
        public DateTime ModificationTime { get; set; }

    }


}
