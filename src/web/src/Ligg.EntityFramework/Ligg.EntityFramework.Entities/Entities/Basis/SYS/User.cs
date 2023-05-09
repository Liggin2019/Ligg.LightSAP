using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ligg.EntityFramework.Entities
{
    [Table("sys_users")]
    public partial class User :  Entity<long>
    {
        public string MasterId { get; set; }

        public string Account { get; set; }
        public bool Status { get; set; }

        public bool Undeleted { get; set; }

        public string Password { get; set; }

        [JsonIgnore]
        public string Salt { get; set; }

        public string Name { get; set; }

        public int? Gender { get; set; }
        public string Birthday { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string WeChat { get; set; }
        public string Qq { get; set; }

        public string Description { get; set; }
        public string ThumbnailPostfix { get; set; }

    }
}
