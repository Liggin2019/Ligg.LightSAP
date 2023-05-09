
using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore;
using Ligg.Uwa.Application;

namespace Ligg.Uwa.Basis.SCC
{
    public class CategoryRepository : TreeRepository<long, Category, DbSetContext>
    {
        public CategoryRepository(IUnitWork<DbSetContext> unitWork, IDbSetRepository<long, Category, DbSetContext> repository) : base(unitWork, repository)
        {
        }


    }
}