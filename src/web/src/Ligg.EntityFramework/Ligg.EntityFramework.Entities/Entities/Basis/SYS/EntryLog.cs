using System.ComponentModel.DataAnnotations.Schema;

namespace Ligg.EntityFramework.Entities
{
    [Table("sys_entrylogs")]
    public partial class EntryLog : Entity<long>
    {
        public int Type  { get; set; }
        public int ApplicationServerId { get; set; }
        public bool State { get; set; }
        public int OperatorType { get; set; }
        public string Account { get; set; }
        public string IpAddress { get; set; }
        public string IpLocation { get; set; }
        public string Browser { get; set; }
        public string OS { get; set; }

        public string UserAgent { get; set; }
        public string Remark { get; set; }

    }
}
