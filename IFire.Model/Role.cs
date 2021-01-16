using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFire.Framework.Abstractions;

namespace IFire.Model {

    [Table("Role")]
    public class Role : Entity<Guid> {
        public string Name { get; set; }
    }
}
