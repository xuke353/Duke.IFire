using System;
using System.Collections.Generic;
using System.Text;

namespace IFire.Framework.Interfaces {
    public interface IEntity<TPrimaryKey> {
        TPrimaryKey Id { get; set; }

        bool IsTransient();
    }
}
