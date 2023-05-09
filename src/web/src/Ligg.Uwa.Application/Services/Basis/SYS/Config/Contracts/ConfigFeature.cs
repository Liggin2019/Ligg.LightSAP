
using Ligg.Uwa.Application.Shared;

namespace Ligg.Uwa.Basis.SYS
{
    public class ConfigFeature
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public string IndexName { get; set; }

        public int Module { get; set; }
        public string ModelName { get; set; }

        public int Type { get; set; }
        public string TypeName { get; set; }
        public long SubType { get; set; }
        public string SubTypeName { get; set; }//code

        public ShowHideOrNone AuthorizationOption { get; set; }
        public int AuthorizationValue { get; set; }
        public bool HasPermissionMark { get; set; }
        public bool HasConsumers { get; set; }
        public PermittedOperatorType ConsumerOption { get; set; }


    }




}



