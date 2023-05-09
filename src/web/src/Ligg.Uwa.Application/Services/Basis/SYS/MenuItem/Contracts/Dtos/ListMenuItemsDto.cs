namespace Ligg.Uwa.Basis.SYS
{
    public  class ListMenuItemsDto
    {
        public string Id { get; set; }

        public string ParentId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }

        public string PageId { get; set; }
         
        public string Icon { get; set; }
        public bool IsBlankTarget { get; set; }
        public bool IsTransparentHeader { get; set; }

        public int? Sequence { get; set; }

        //not Maped
        public string Url { get; set; } //outlink url, vue view, winfor view
        public string Style { get; set; }

        public string Code { get; set; }
        public string Redirect { get; set; }



    }


}
