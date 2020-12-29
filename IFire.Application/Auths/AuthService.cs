using System;
using System.Threading.Tasks;
using IFire.Application.Auths.Dto;
using IFire.Auth.Abstractions;
using IFire.Domain.RepositoryIntefaces;
using IFire.Framework.Extensions;
using IFire.Framework.Helpers;
using IFire.Framework.Interfaces;
using IFire.Framework.Result;
using IFire.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IFire.Application.Auths {

    public class AuthService : IFireAppServiceBase, IAuthService {
        private readonly IConfigProvider _configProvider;
        private readonly IRepository<Account, int> _accountRepository;
        private readonly IRepository<LoginLog, int> _loginLogRepository;
        private readonly IPasswordHandler _passwordHandler;
        private readonly IRepository<AccountAuthInfo, int> _accountAuthInfoRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly IRepository<Role, Guid> _roleRepository;
        private readonly IpHelper _ipHelper;

        public AuthService(IConfigProvider configProvider,
            IRepository<Account, int> accountRepository,
            IRepository<LoginLog, int> loginLogRepository,
            IPasswordHandler passwordHandler,
            IRepository<AccountAuthInfo, int> accountAuthInfoRepository,
            ILogger<AuthService> logger,
            IRepository<Role, Guid> roleRepository,
            IpHelper ipHelper) {
            _configProvider = configProvider;
            _accountRepository = accountRepository;
            _loginLogRepository = loginLogRepository;
            _passwordHandler = passwordHandler;
            _accountAuthInfoRepository = accountAuthInfoRepository;
            _logger = logger;
            _roleRepository = roleRepository;
            _ipHelper = ipHelper;
        }

        public async Task<LoginResult> Login(LoginInput input) {
            var loginResult = new LoginResult() {
                Username = input.Username,
                LoginTime = DateTime.Now
            };
            //检测
            var checkResult = await Check(input, loginResult);
            if (checkResult.Successful) {
                loginResult.Success = true;
                //更新认证信息并返回登录结果
                await UpdateAuthInfo(loginResult, input);
            } else {
                loginResult.Success = false;
                loginResult.Error = checkResult.Msg;
            }
            await SaveLog(loginResult);
            return loginResult;
        }

        /// <summary>
        /// 登录处理
        /// </summary>
        private async Task<IResultModel> Check(LoginInput model, LoginResult resultModel) {
            //查询账户
            var account = await _accountRepository.FirstOrDefaultAsync(t => t.Username.Equals(model.Username));
            if (account == null)
                return ResultModel.Failed("账户不存在");

            //设置账户编号和名称
            resultModel.UserId = account.Id;
            resultModel.Name = account.Name;
            resultModel.AccountType = account.Type;
            //检测密码
            var password = _passwordHandler.Encrypt(account.Username, model.Password);
            if (!account.Password.Equals(password))
                return ResultModel.Failed("密码错误");

            //检测账户
            var accountCheckResult = account.Check();
            if (!accountCheckResult.Successful)
                return ResultModel.Failed(accountCheckResult.Msg);

            return ResultModel.Success();
        }

        /// <summary>
        /// 保存日志
        /// </summary>
        private async ValueTask SaveLog(LoginResult result) {
            //保存日志，不能抛出异常以免影响登录本身的功能
            try {
                var entity = new LoginLog {
                    UserId = result.UserId,
                    Username = result.Username,
                    Error = result.Error,
                    LoginTime = result.LoginTime,
                    Success = result.Success,
                    IP = _ipHelper.IP,
                    UserAgent = _ipHelper.UserAgent
                };
                await _loginLogRepository.InsertAsync(entity);
            } catch (Exception ex) {
                _logger.LogError("登录日志存储失败：{@ex}", ex);
            }
        }

        /// <summary>
        /// 更新账户认证信息
        /// </summary>
        private async Task UpdateAuthInfo(LoginResult resultModel, LoginInput loginModel) {
            resultModel.RefreshToken = GenerateRefreshToken();

            var authInfo = new AccountAuthInfo {
                UserId = resultModel.UserId,
                LoginTime = resultModel.LoginTime.ToTimestamp(),
                RefreshToken = resultModel.RefreshToken,
                RefreshTokenExpiredTime = DateTime.UtcNow.AddDays(7)//默认刷新令牌有效期7天
            };
            var config = _configProvider.Get<AuthConfig>();

            //设置过期时间
            if (config.Jwt.RefreshTokenExpires > 0) {
                authInfo.RefreshTokenExpiredTime = DateTime.UtcNow.AddDays(config.Jwt.RefreshTokenExpires);
            }

            var entity = await _accountAuthInfoRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(t => t.UserId == resultModel.UserId);
            if (entity != null) {
                authInfo.Id = entity.Id;
                await _accountAuthInfoRepository.UpdateAsync(authInfo);
            } else {
                await _accountAuthInfoRepository.InsertAsync(authInfo);
            }
        }

        /// <summary>
        /// 生成刷新令牌
        /// </summary>
        /// <returns></returns>
        private static string GenerateRefreshToken() {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public Task GetAuthInfo() {
            _roleRepository.InsertAsync(new Role { Name = "张三" });
            return Task.CompletedTask;
        }

        public Task<AccountAuthInfo> GetAuthInfo(int userId) {
            throw new NotImplementedException();
        }

        public Task<LoginResult> RefreshToken(string refreshToken) {
            throw new NotImplementedException();
        }
    }
}
