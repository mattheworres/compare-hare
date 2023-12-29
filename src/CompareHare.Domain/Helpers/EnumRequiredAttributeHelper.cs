using System.ComponentModel.DataAnnotations;

namespace CompareHare.Domain.Helpers
{
    public static class EnumRequiredAttributeHelper
    {
        public static bool MarkedAsRequired(object value)
        {
            return EnumValueHasAttributeHelper<RequiredAttribute>.HasAttribute(value);
        }
    }
}
