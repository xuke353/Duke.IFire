﻿using System;

namespace IFire.Data.EFCore.Uow {

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public class UnitOfWorkAttribute : Attribute {
        public bool IsDisabled { get; set; }
    }
}
