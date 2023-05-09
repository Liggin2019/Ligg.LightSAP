using System.ComponentModel.DataAnnotations.Schema;

namespace Ligg.EntityFramework.Entities
{
    [Table("sys_menuitems")]
    public  class MenuItem :  Tree<long>
    {
        public int Type { get; set; }
        public string Code { get; set; }
        public string Redirect { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string PageId { get; set; }
        public bool IsBlankTarget { get; set; }
        public string UrlParams { get; set; }//for systematic, cust page, vue route
        public string Style { get; set; }
        public int? Sequence { get; set; }

    }

}
