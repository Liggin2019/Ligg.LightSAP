using System;

namespace Ligg.Uwa.Basis.SCC
{
    public  class ManageArticlesDto
    {

        public ManageArticlesDto()
        {
            Name = string.Empty;
            MasterCascadePath = "暂无";
        }
        public string Id { get; set; }
        public string MasterId { get; set; }

        public int Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public string Note { get; set; }

        public int? Status { get; set; }
        public int? Sequence { get; set; }
        public DateTime ModificationTime { get; set; }
        public DateTime CreationTime { get; set; }

        public int HasThumbnail { get; set; }
        public int HasImage { get; set; }
        public int HasVideo { get; set; }

        public int AttachedFilesNum { get; set; }
        public int AttachedImagesNum { get; set; }
        public string MasterCascadePath { get; set; }
    }


}
