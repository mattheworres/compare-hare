using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CompareHare.Domain.Extensions;

namespace CompareHare.Domain.Helpers
{
    public static class EnumDisplayHelper
    {
        public static string GetDisplayName(object value) => GetDisplayName(value, value.GetType());

        public static string GetDisplayName(object value, Type enumType)
        {
            if (value == null) return null;

            var field = enumType.GetField(Enum.GetName(enumType, value));
            var attr = (DisplayAttribute)Attribute.GetCustomAttribute(field, typeof(DisplayAttribute));
            return attr?.GetName() ?? Enum.ToObject(enumType, value).ToString().SpaceBeforeCapitalLetters().ToTitleCase();
        }

        public static int? GetOrderNumber(object value, Type enumType)
        {
            if (value == null) return null;

            var field = enumType.GetField(Enum.GetName(enumType, value));
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
    }
}
