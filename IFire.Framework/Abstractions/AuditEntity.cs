using System;

namespace IFire.Framework.Abstractions {

    public class AuditEntity : CreationEntity {
        public int? ModifierId { get; set; }

        public string ModifierName { get; set; }

        public DateTime? ModifyTime { get; set; }
    }
}
