using System;

namespace CompareHare.Api.BackgroundJobs.Attributes
{
    public class JobOrderAttribute : Attribute
    {
        public int Order { get; }
        public JobOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
