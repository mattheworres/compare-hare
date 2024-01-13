using CompareHare.Domain.Attributes;

namespace CompareHare.Domain.Helpers
{
    public static class EnumFrontEndConstantHelper
    {
        public static string GetConstant(object value)
        {
            return GetConstant(value, value.GetType());
        }

        public static string GetConstant(object value, Type enumType)
        {
            if (value == null) return "";

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var field = enumType.GetField(Enum.GetName(enumType, value));
            var attr = (FrontEndConstantAttribute)Attribute.GetCustomAttribute(field, typeof(FrontEndConstantAttribute));
            return attr?.Constant ?? "";
        }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8604 // Possible null reference argument
    }
}
