using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace IFire.Data.EFCore.Uow {

    public interface IIFireUnitOfWork {
        IFireDbContext CurrentDbContext { get; }

        IDbContextTransaction Begin();

        int SaveChanges();

        Task<int> SaveChangesAsync();

        void Complete(IDbContextTransaction transaction);

        Task CompleteAsync(IDbContextTransaction transaction);

        void RollBackChanges();

        void Dispose();
    }
}
