using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFire.Framework.Interfaces {

    public interface ISoftDelete<TKey> {
        bool IsDeleted { get; set; }

        TKey DeleterId { get; set; }

        string DeleterName { get; set; }

        DateTime? DeletionTime { get; set; }
    }

    public interface ISoftDelete : ISoftDelete<int?> {
    }
}
