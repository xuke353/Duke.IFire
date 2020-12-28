using AutoMapper;
using IFire.Auth.Abstractions;
using IFire.Data.EFCore.Uow;

namespace IFire.Application {

    [UnitOfWork]
    public class IFireAppServiceBase {
        public IIFireUnitOfWork CurrentUnitOfWork { get; set; }

        public IMapper ObjectMapper { get; set; }

        public IIFireSession IFireSession { get; set; }
    }
}
