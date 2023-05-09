using System.ComponentModel.DataAnnotations.Schema;

namespace Ligg.EntityFramework.Entities
{
    [Table("sys_actionlogs")]
    public partial class ActionLog :  Entity<long>
    {
        public int Type { get; set; }

        public string Client { get; set; }

        public int ApplicationServerId { get; set; }

        public int OperatorType{ get; set; }
        public string IpAddress { get; set; }
        public string IpLocation { get; set; }
        public string Url { get; set; }
        public string RequestString { get; set; }

        public string? Exception { get; set; }
        public long? RunningDuration { get; set; }

        public bool State { get; set; }




    }


}
