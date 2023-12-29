using System;

namespace CompareHare.Domain.Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    public class FrontEndConstantAttribute : Attribute
    {
        public FrontEndConstantAttribute(string constant) {
            Constant = constant;
        }

        public string Constant { get; }
    }
}
