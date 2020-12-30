using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IFire.Application.AuditInfos.Dto;
using IFire.Domain.RepositoryIntefaces;
using IFire.Framework.Abstractions;
using IFire.Models;

namespace IFire.Application.AuditInfos {

    public class AuditInfoService : IFireAppServiceBase, IAuditInfoService {
        private readonly IRepository<AuditInfo, int> _auditInfoRepository;

        public AuditInfoService(IRepository<AuditInfo, int> auditInfoRepository) {
            _auditInfoRepository = auditInfoRepository;
        }

        public Task Add(AuditInfo info) {
            return _auditInfoRepository.InsertAsync(info);
        }

        public Page<GetAuditInfoOutput> Query(GetAuditInfoInput input) {
            var result = _auditInfoRepository.GetAll();
            var pageResult = result.PageAndOrderBy(input);
            var output = ObjectMapper.Map<List<GetAuditInfoOutput>>(pageResult.ToList());
            return new Page<GetAuditInfoOutput>(input, result.Count(), output);
        }
    }
}
