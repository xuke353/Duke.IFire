using System;
using System.Collections.Generic;
using System.Text;

namespace IFire.Framework.Interfaces {

    public interface IConfigProvider {

        TConfig Get<TConfig>(string section = "") where TConfig : IConfig, new();
    }
}
