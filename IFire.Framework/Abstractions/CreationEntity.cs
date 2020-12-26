using System;

namespace IFire.Framework.Abstractions {

    public class CreationEntity : CreationEntity<int> { }

    public class CreationEntity<TKey> : Entity<TKey> {
        public TKey CreatorId { get; set; }

        public string CreatorName { get; set; }

        public virtual DateTime? CreateTime { get; set; }
    }
}
