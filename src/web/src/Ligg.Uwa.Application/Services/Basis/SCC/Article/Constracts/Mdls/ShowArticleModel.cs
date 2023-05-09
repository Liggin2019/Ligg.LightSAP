using System;

namespace Ligg.Uwa.Basis.SCC
{
    public  class ShowArticleModel
    {
        public string Id { get; set; }

        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }

        public string ModificationTime { get; set; }
        public string HumanizedModificationTime { get; set; }
        public string CreationTime { get; set; }

        //public int AttachedFilesNum { get; set; }
        public string MasterCascadePath { get; set; }

    }


}
