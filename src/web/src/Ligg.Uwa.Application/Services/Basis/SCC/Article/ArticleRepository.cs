using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore;
using Ligg.Infrastructure.Extensions;
using Ligg.Uwa.Application;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Application.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Ligg.Uwa.Basis.SCC
{
    public class ArticleRepository : CommonRepository<long, Article, DbSetContext>
    {
        public ArticleRepository(IUnitWork<DbSetContext> unitWork, IDbSetRepository<long, Article, DbSetContext> repository) : base(unitWork, repository)
        {
        }








    }
}