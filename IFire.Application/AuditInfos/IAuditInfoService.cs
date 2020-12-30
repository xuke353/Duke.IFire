﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFire.Application.AuditInfos.Dto;
using IFire.Framework.Abstractions;
using IFire.Models;

namespace IFire.Application.AuditInfos {

    public interface IAuditInfoService {

        /// <summary>
        /// 添加审计信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task Add(AuditInfo info);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Page<GetAuditInfoOutput> Query(GetAuditInfoInput input);
    }
}
