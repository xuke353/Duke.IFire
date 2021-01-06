using System;

namespace IFire.Framework.Attributes {

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class SectionAttribute : Attribute {
        public string Name { get; set; }

        public SectionAttribute(string name) {
            Name = name;
        }
    }
}
