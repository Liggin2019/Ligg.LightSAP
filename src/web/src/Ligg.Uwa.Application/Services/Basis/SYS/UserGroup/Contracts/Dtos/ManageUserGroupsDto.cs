﻿
using System;

namespace Ligg.Uwa.Basis.SYS
{
    public class ManageUserGroupsDto
    {
        public string Id { get; set; }

        public int Type { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public int? Builtin { get; set; }

        public int UserNum { get; set; }
        public int? Sequence { get; set; }

        public int Status { get; set; }

        public DateTime ModificationTime { get; set; }


    }

}
