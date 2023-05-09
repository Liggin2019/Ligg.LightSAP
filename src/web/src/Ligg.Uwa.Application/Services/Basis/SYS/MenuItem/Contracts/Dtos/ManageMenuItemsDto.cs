using System;

namespace Ligg.Uwa.Basis.SYS
{
    public  class ManageMenuItemsDto
    {
        public string Id { get; set; }
        public string TopParentId { get; set; }
        public string ParentId { get; set; }
        public int MenuType { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string PageId { get; set; }
        public string Icon { get; set; }
        public int IsBlankTarget { get; set; }
        public string UrlParams { get; set; }
        public string Code { get; set; }
        public string Redirect { get; set; }
        public int? Sequence { get; set; }
        public int Status { get; set; }

        public DateTime ModificationTime { get; set; }


    }


}
