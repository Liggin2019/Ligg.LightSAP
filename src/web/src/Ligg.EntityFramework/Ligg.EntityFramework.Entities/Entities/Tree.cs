using Ligg.EntityFramework.Entities.Helpers;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ligg.EntityFramework.Entities
{

    public  class Tree<TKey> : Entity<TKey>
    {
        [Description("父节点Id")]

        [JsonConverter(typeof(StringJsonConverter))]
        public string ParentId { get; set; }

        public string Name { get; set; }

        [Description("节点路径No")]
        public string CascadedNo { get; set; }

        //*如果服务器存储资源多， 可以使用以下2项；可以减少内存开销和提升速度
        //[Description("节点路径Id")]
        //[NotMapped]
        //public string CascadedId { get; set; }

        ////[NotMapped]
        //[Description("节点路径Name")]
        //public string CascadedName { get; set; }

        public bool Status { get; set; }

    }
}

