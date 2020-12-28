using IFire.Models;
using Microsoft.EntityFrameworkCore;

namespace IFire.Data.EFCore {

    public class IFireDbContext : DbContext {
        public  DbSet<Account> Accounts { get; set; }
        public  DbSet<AccountAuthInfo> AccountAuthInfos { get; set; }
        public  DbSet<LoginLog> LoginLogs { get; set; }
        public IFireDbContext(DbContextOptions<IFireDbContext> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
    }
}
