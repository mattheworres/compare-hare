

using System.ComponentModel.DataAnnotations;

using CompareHare.Domain.Extensions;

namespace CompareHare.Domain.Helpers
{
    public static class EnumDisplayHelper
    {
        public static string GetDisplayName(object value) => GetDisplayName(value, value.GetType());

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        public static string GetDisplayName(object value, Type enumType)
        {
            if (value == null) return "";

            var name = Enum.GetName(enumType, value) ?? "";
            var field = enumType.GetField(name);

            var attr = (DisplayAttribute)Attribute.GetCustomAttribute(field, typeof(DisplayAttribute));
            return attr?.GetName() ?? Enum.ToObject(enumType, value).ToString().SpaceBeforeCapitalLetters().ToTitleCase();
        }

        public static int? GetOrderNumber(object value, Type enumType)
        {
            if (value == null) return null;

            var name = Enum.GetName(enumType, value) ?? "";
            var field = enumType.GetField(name);
            var attr = (DisplayAttribute)Attribute.GetCustomAttribute(field, typeof(DisplayAttribute));
            return attr?.GetOrder();
        }

        public static IEnumerable<int> GetOrderedValues(Type enumType)
        {
            var orderableValues = new List<Tuple<int, int>>();

            foreach (int value in Enum.GetValues(enumType))
                orderableValues.Add(new Tuple<int, int>(GetOrderNumber(value, enumType) ?? int.MaxValue, value));

            var orderedValues = orderableValues.OrderBy(t => t.Item1).Select(t => t.Item2).ToList();

            return orderedValues;
        }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }
}
