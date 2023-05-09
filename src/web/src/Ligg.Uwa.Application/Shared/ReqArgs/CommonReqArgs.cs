using Ligg.EntityFramework.Entities.Helpers;
using Newtonsoft.Json;
using System;

namespace Ligg.Uwa.Application.Shared
{
    public class CommonReqArgs
    {
        public int? Type { get; set; }

        [JsonConverter(typeof(StringJsonConverter))]
        public long? LongType { get; set; }

        public string Mark { get; set; }
        public string Text { get; set; }

        //public string Name { get; set; }
        //public string Description { get; set; }
        public int? Status { get; set; }
        public int? Deleted { get; set; }


        public DateTime? StartCreationTime { get; set; }
        public DateTime? EndCreationTime { get; set; }

        public DateTime? StartModificationTime { get; set; }
        public DateTime? EndModificationTime { get; set; }


    }



}
