using Ligg.EntityFramework.Entities.Helpers;
using Newtonsoft.Json;
using System;

namespace Ligg.Uwa.Basis.SYS
{
    public class ManageUsersDto
    {

        public ManageUsersDto()
        {
            Account = string.Empty;
            Name = string.Empty;
            MasterCascadePath = "暂无";
        }

        public string Id { get; set; }

        public string Account { get; set; }

        public string Name { get; set; }
        public string Mobile { get; set; }

        public int? Status { get; set; }
        public DateTime ModificationTime { get; set; }

        public string MasterId { get; set; }
        public string MasterCascadePath { get; set; }

        public int HasThumbnail { get; set; }
        



    }

}
