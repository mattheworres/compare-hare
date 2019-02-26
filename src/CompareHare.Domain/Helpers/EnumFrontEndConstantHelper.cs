using System;
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
            if (value == null) return null;

            var field = enumType.GetField(System.Enum.GetName(enumType, value));
            var attr = (FrontEndConstantAttribute)System.Attribute.GetCustomAttribute(field, typeof(FrontEndConstantAttribute));
            return attr?.Constant;
        }
    }
}
