using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore;
using Ligg.Uwa.Application;

namespace Ligg.Uwa.Basis.SYS
{
    public class OrganizationRepository : TreeRepository<long, Organization, DbSetContext>
    {
        public OrganizationRepository(IUnitWork<DbSetContext> unitWork, IDbSetRepository<long, Organization, DbSetContext> repository) : base(unitWork, repository)
        {
        }


    }
}