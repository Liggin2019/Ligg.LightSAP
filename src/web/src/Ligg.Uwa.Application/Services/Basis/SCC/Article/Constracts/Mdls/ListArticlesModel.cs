using System;

namespace Ligg.Uwa.Basis.SCC
{
    public  class ListArticlesModel
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }

        public string CreationTime { get; set; }
        public string ModificationTime { get; set; }
        public string HumanizedModificationTime { get; set; }

        public string HasThumbnail { get; set; }
        public string HasImage { get; set; }
        public string HasVideo { get; set; }

        public string AttachedFilesNum { get; set; }
        public string AttachedImagesNum { get; set; }
        public string MasterCascadePath { get; set; }
    }


}
