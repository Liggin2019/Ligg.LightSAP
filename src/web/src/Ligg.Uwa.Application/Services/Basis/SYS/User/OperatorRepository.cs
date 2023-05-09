using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore;
using Ligg.Uwa.Application;

namespace Ligg.Uwa.Basis.SYS
{
    public class OperatorRepository : CommonRepository<long, Operator, DbSetContext>
    {
        public OperatorRepository(IUnitWork<DbSetContext> unitWork, IDbSetRepository<long, Operator, DbSetContext> repository) : base(unitWork, repository)
        {
        }



    }
}