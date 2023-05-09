
namespace Ligg.Uwa.Basis.SYS
{
    public  class AddEditTenantDto
    {
        public string Id { get; set; }
        public int Type { get; set; }
        public string Key { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int? Sequence { get; set; }
        public int Status { get; set; }

    }


}
