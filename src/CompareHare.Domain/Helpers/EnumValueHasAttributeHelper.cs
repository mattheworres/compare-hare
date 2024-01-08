

namespace CompareHare.Domain.Helpers
{
    public static class EnumValueHasAttributeHelper<TAttribute>
        where TAttribute : Attribute
    {
        public static bool HasAttribute(object value)
        {
            return HasAttribute(value, value.GetType());
        }

        public static bool HasAttribute(object value, Type enumType)
        {
            if (value == null) return false;

            var field = enumType.GetField(System.Enum.GetName(enumType, value));
            var attr = (TAttribute)System.Attribute.GetCustomAttribute(field, typeof(TAttribute));
            return attr != null;
        }
    }
}
