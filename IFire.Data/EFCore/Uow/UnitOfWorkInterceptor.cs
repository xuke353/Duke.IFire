

using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using IFire.Framework.Extensions;

namespace IFire.Data.EFCore.Uow {
    /// <summary>
    /// 工作单元拦截器
    /// </summary>
    public class UnitOfWorkInterceptor : IInterceptor {
        private readonly IIFireUnitOfWork _iFireUnitOfWork;

        public UnitOfWorkInterceptor(IIFireUnitOfWork iFireUnitOfWork) {
            _iFireUnitOfWork = iFireUnitOfWork;
        }

        public void Intercept(IInvocation invocation) {
            MethodInfo method;
            try {
                method = invocation.MethodInvocationTarget;
            } catch {
                method = invocation.GetConcreteMethod();
            }

            var unitOfWorkAttr = method.GetAttribute<UnitOfWorkAttribute>();
            if (unitOfWorkAttr == null || unitOfWorkAttr.IsDisabled) {
                //No need to a uow
                invocation.Proceed();
                return;
            }

            //No current uow, run a new one
            PerformUow(invocation);
        }

        private void PerformUow(IInvocation invocation) {
            var method = invocation.Method;
            if (method.ReturnType == typeof(Task) ||
                (method.ReturnType.GetTypeInfo().IsGenericType &&
                method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))) {
                PerformAsyncUow(invocation);
            } else {
                PerformSyncUow(invocation);
            }
        }

        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="invocation"></param>
        private void PerformSyncUow(IInvocation invocation) {
            using (var uow = _iFireUnitOfWork.Begin()) {
                invocation.Proceed();
                _iFireUnitOfWork.Complete(uow);
            }
        }

        /// <summary>
        /// 异步
        /// </summary>
        /// <param name="invocation"></param>
        private void PerformAsyncUow(IInvocation invocation) {
            var uow = _iFireUnitOfWork.Begin();

            try {
                invocation.Proceed();
            } catch {
                uow.Dispose();
                throw;
            }

            if (invocation.Method.ReturnType == typeof(Task)) {
                invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
                    (Task)invocation.ReturnValue,
                    async () => await _iFireUnitOfWork.CompleteAsync(uow),
                    exception => uow.Dispose()
                );
            } else //Task<TResult>
              {
                invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                    invocation.Method.ReturnType.GenericTypeArguments[0],
                    invocation.ReturnValue,
                    async () => await _iFireUnitOfWork.CompleteAsync(uow),
                    exception => uow.Dispose()
                );
            }
        }
    }
}
