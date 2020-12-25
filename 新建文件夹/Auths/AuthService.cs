using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IFire.Application.Auths.Dto;
using IFire.Framework.Interfaces;
using IFire.Framework.Result;
using IFire.Models;

namespace IFire.Application.Auths {
    public class AuthService : IAuthService {
        private readonly IConfigProvider _configProvider;

        public AuthService(IConfigProvider configProvider) {
            _configProvider = configProvider;
        }

        public  Task<LoginResult> Login(LoginInput input) {
            return (Task<LoginResult>)Task.CompletedTask;
            //var config = _configProvider.Get<AuthConfig>("Auth");
            //if (!config.LoginMode.UserName)
            //    return ResultModel.Failed("不允许使用用户名的登录方式");

            ////检测验证码
            //var verifyCodeCheckResult = _verifyCodeProvider.Check(model);
            //if (!verifyCodeCheckResult.Successful)
            //    return ResultModel.Failed(verifyCodeCheckResult.Msg);

            ////查询账户
            //var account = await _repository.GetByUserName(model.UserName, model.AccountType, resultModel.TenantId);
            //if (account == null)
            //    return ResultModel.Failed("账户不存在");

            ////设置账户编号和名称
            //resultModel.AccountId = account.Id;
            //resultModel.Name = account.Name;

            ////检测密码
            //var password = _passwordHandler.Encrypt(account.UserName, model.Password);
            //if (!account.Password.Equals(password))
            //    return ResultModel.Failed("密码错误");

            ////检测账户
            //var accountCheckResult = account.Check();
            //if (!accountCheckResult.Successful)
            //    return ResultModel.Failed(accountCheckResult.Msg);
        }

        public Task<IResultModel> GetAuthInfo() {
            throw new NotImplementedException();
        }

        public Task<AccountAuthInfo> GetAuthInfo(int userId) {
            throw new NotImplementedException();
        }

        public Task<LoginResult> RefreshToken(string refreshToken) {
            throw new NotImplementedException();
        }
    }
}
