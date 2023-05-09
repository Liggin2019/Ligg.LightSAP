
using Ligg.EntityFramework.Entities.Helpers;
using Newtonsoft.Json;
using System;

namespace Ligg.Uwa.Basis.SYS
{
    public  class PublicTenant
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public long Id { get { return 0; } }
        public int Type { get { return 0; } }
        public string Key { get { return "Ligg"; } }
        public string Code { get { return "public"; } }
        public string ShortName { get { return "理格科技"; } }
        public string Name { get { return "理格科技有限公司"; } }
        public string Description { get { return "有理想, 有格局"; } }

        public string ThumbnailPostfix { get { return ".png"; } }
        public string ImagePostfix { get { return ".png"; } }

        public bool HasIco { get { return true; } }

        public bool IsPublic { get { return true; } }
        public bool IsDefault { get; set; }
        public int? Sequence { get { return 0; } }
        public bool Status { get { return true; } }

        public string CreatorId { get{ return "0"; } }

        public DateTime CreationTime { get { return new DateTime(2022,1,1); } }

        public string LastModifierId { get{ return "0"; } }

        public DateTime? ModificationTime { get{ return DateTime.Now; } }

    }


}
