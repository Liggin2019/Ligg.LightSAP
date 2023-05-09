
using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore;
using Ligg.Uwa.Application;

namespace Ligg.Uwa.Basis.SYS
{
    public class MenuItemRepository : TreeRepository<long, MenuItem, DbSetContext>
    {
        public MenuItemRepository(IUnitWork<DbSetContext> unitWork, IDbSetRepository<long, MenuItem, DbSetContext> repository) : base(unitWork, repository)
        {
        }


    }
}