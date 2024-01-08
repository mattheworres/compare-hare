
using CompareHare.Domain.Helpers;

namespace CompareHare.Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string DisplayName(this Enum value) => EnumDisplayHelper.GetDisplayName(value);
    }
}
