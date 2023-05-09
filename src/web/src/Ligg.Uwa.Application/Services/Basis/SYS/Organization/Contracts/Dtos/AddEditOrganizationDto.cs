using Ligg.EntityFramework.Entities.Helpers;
using Newtonsoft.Json;
using System;

namespace Ligg.Uwa.Basis.SYS
{
    public  class AddEditOrganizationDto
    {
        public string Id { get; set; }

        public string ParentId { get; set; }

        [JsonConverter(typeof(StringJsonConverter))]
        public long Type { get; set; }

        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }

        public string Description { get; set; }

        public int? Sequence { get; set; }

        public string Owner { get; set; }

        public int Status { get; set; }

        //[NotMapped]
        public string ParentName { get; set; }

    }


}
