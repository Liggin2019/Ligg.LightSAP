
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ligg.EntityFramework.Entities
{
    [Table("sys_operators")]
    public partial class Operator :  Entity<long>
    {
        public int Type { get; set; }
        public string ActorId { get; set; }
        public int? LoginCount { get; set; }
        public DateTime? FirstVisit { get; set; }
        public DateTime? PreviousVisit { get; set; }
        public DateTime? LastVisit { get; set; }

    }
}
