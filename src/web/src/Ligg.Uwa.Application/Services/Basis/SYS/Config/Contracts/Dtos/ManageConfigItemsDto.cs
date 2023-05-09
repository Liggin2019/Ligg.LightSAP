using Ligg.EntityFramework.Entities.Helpers;
using Newtonsoft.Json;
using System;

namespace Ligg.Uwa.Basis.SYS
{

    public class ManageConfigItemsDto
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public string Id { get; set; }

        public string Key { get; set; }
        //public string Value { get; set; }
        public string Attribute { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Authorization { get; set; }
        public string PermissionMark { get; set; }

        public string ConsumerNum { get; set; }

        public string Style { get; set; }
        public int IsDefault { get; set; }

        public int? Sequence { get; set; }
        public int Status { get; set; }

 
        public DateTime ModificationTime { get; set; }

    }




}



