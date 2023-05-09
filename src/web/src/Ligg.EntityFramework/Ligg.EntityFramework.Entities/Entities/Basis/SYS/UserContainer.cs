
using System.ComponentModel.DataAnnotations.Schema;
namespace Ligg.EntityFramework.Entities
{
    [Table("sys_usercontainers")]
    public  class UserContainer:  Entity<long>
    {
        public int Type { get; set; }

        public string UserId { get; set; }

        public string ContainerId { get; set; }

    }



}
