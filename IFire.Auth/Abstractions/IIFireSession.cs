namespace IFire.Auth.Abstractions {

    public interface IIFireSession<TKey> {

        /// <summary>
        /// 用户Id
        /// </summary>
        TKey UserId { get; }

        /// <summary>
        /// 由字母或数字组成的用户名称，以标明用户的身份
        /// </summary>
        string Username { get; }

        /// <summary>
        /// 名字
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 是否认证
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public long LoginTime { get; }
    }

    public interface IIFireSession : IIFireSession<int?> {
    }
}
