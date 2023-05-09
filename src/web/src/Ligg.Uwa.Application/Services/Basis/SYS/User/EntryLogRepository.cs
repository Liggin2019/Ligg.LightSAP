using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore;
using Ligg.Uwa.Application;

namespace Ligg.Uwa.Basis.SYS
{
    public class EntryLogRepository : CommonRepository<long, EntryLog, DbSetContext>
    {

        public EntryLogRepository(IUnitWork<DbSetContext> unitWork, IDbSetRepository<long, EntryLog, DbSetContext> repository) : base(unitWork, repository)
        {
        }




    }
}