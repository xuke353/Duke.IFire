﻿using System.Collections.Generic;
using IFire.Application.Auths.Web;
using IFire.Model.Enums;

namespace IFire.Application.Auths.Dto {

    public class AuthInfo {

        /// <summary>
        /// 账户标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public AccountType Type { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜单列表
        /// </summary>
        public IList<AccountMenuItem> Menus { get; set; }

        /// <summary>
        /// 详情信息(用于扩展登录对象信息)
        /// </summary>
        public object Details { get; set; }
    }
}
