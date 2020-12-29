using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace IFire.Framework.Interceptors {

    public class IFireExceptionFilter : IExceptionFilter {
        private readonly ILogger<IFireExceptionFilter> _logger;

        public IFireExceptionFilter(ILogger<IFireExceptionFilter> logger) {
            _logger = logger;
        }

        public void OnException(ExceptionContext context) {
        }
    }
}
