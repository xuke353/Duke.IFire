using Microsoft.EntityFrameworkCore;

namespace IFire.Data.EFCore {

    public class IFireDbContext : DbContext {
        public IFireDbContext(DbContextOptions<IFireDbContext> options) : base(options) { }
    }
}
