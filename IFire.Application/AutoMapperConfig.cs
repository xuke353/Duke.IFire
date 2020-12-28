using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace IFire.Application {
    public class AutoMapperConfig : Profile {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public AutoMapperConfig() {
        }
    }
}
