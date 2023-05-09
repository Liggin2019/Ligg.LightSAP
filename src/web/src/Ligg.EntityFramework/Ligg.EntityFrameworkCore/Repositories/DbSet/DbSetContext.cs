using Ligg.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

#nullable disable

namespace Ligg.EntityFrameworkCore
{
    public partial class DbSetContext : Microsoft.EntityFrameworkCore.DbContext
    {
        //private ILoggerFactory _LoggerFactory;
        public DbSetContext(DbContextOptions<DbSetContext> options/*, ILoggerFactory loggerFactory*/) : base(options)
        {
            //_LoggerFactory = loggerFactory;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);  //允许打印参数
            //optionsBuilder.UseLoggerFactory(_LoggerFactory);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<DataPrivilegeRule>().HasKey(x => new { x.Id });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        public virtual DbSet<EntryLog> EntryLogs { get; set; }
        public virtual DbSet<ActionLog> ActionLogs { get; set; }

        public virtual DbSet<Tenant> Tenants { get; set; }

        public virtual DbSet<Config> Configs { get; set; }
        public virtual DbSet<ConfigItem> ConfigItems { get; set; }

        public virtual DbSet<MenuItem> MenuItems { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Operator> Operators { get; set; }
        public virtual DbSet<UserContainer> UserContainers { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }


        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Article> Articles { get; set; }

    }
}
