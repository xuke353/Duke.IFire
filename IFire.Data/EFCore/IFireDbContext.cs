using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IFire.Auth.Abstractions;
using IFire.Model;
using IFire.Model.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IFire.Data.EFCore {

    public class IFireDbContext : DbContext {
        public IIFireSession IFireSession { get; set; }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountAuthInfo> AccountAuthInfos { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<AuditInfo> AuditInfos { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        public IFireDbContext(IIFireSession iFireSession, DbContextOptions<IFireDbContext> options) : base(options) {
            IFireSession = iFireSession;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Account>().HasData(new Account[]
           {
                new Account {
                    Id = 1,
                    Name = "管理员",
                    Username = "admin",
                    Password = "F0DD923B1DB060E4DFB4AA10CB855FBA",//a123456
                    IsLock = false,
                    Deleted = false,
                    Status = AccountStatus.激活,
                    Type = AccountType.管理员
                }
           });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission[]
            {
                new RolePermission {
                    Id = 1,
                    PermissionCode ="WeatherForecast_Get_Get",
                    RoleId = 1
                },
                new RolePermission {
                    Id = 2,
                    PermissionCode ="Permission_Tree_Get",
                    RoleId = 1
                },
                new RolePermission {
                    Id = 3,
                    PermissionCode ="Auth_AuthInfo_Get",
                    RoleId = 1
                }
            });
            modelBuilder.Entity<AccountRole>().HasData(new AccountRole[]
            {
                new AccountRole {
                    Id = 1,
                    UserId = 1,
                    RoleId = 1
                },
            });
            base.OnModelCreating(modelBuilder);
        }

        #region EntityAuditing

        public override int SaveChanges() {
            try {
                ApplyConcepts();
                return base.SaveChanges(); ;
            } catch (DbUpdateConcurrencyException ex) {
                throw new DbUpdateConcurrencyException(ex.Message, ex);
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
            try {
                ApplyConcepts();
                return base.SaveChangesAsync(cancellationToken);
            } catch (DbUpdateConcurrencyException ex) {
                throw new DbUpdateConcurrencyException(ex.Message, ex);
            }
        }

        private void ApplyConcepts() {
            var username = IFireSession.Username;
            var userId = IFireSession.UserId;
            foreach (var entry in ChangeTracker.Entries().ToList()) {
                switch (entry.State) {
                    case EntityState.Added:
                        EntityAuditingHelper.SetCreationAuditProperties(entry, username, userId);
                        break;

                    case EntityState.Modified:
                        EntityAuditingHelper.SetModificationAuditProperties(entry, username, userId);
                        break;

                    case EntityState.Deleted:
                        EntityAuditingHelper.SetDeletionAuditProperties(entry, username, userId);
                        break;
                }
            }
        }

        #endregion EntityAuditing
    }
}
