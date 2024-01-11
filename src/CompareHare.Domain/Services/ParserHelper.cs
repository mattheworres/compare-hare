using System.Globalization;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using CompareHare.Domain.Services.Interfaces;
using Serilog;

namespace CompareHare.Domain.Services
{
    public class ParserHelper : IParserHelper
    {
        private const string SPACE = " ";
        private const string CLASS = "class";
        private const string NUMBER_PATTERN = @"([0-9])+";
        private const string FLOAT_PATTERN = @"[+-]?([0-9]+([.][0-9]*)?|[.][0-9]+)";

        public bool ElementHasClass(IElement element, string className, bool caseSensitive = true)
        {
            var listOfClasses = _GetClassValue(element).Split(SPACE);

            if (caseSensitive) return listOfClasses.Any(x => x == className);

            return listOfClasses.Any(x => x.ToLowerInvariant() == className.ToLowerInvariant());
        }

        public IEnumerable<string> GetClasses(IElement element)
        {
            return _GetClassValue(element).Split(SPACE);
        }

        public string GetElementClassStartingWith(IElement element, string startingNeedle)
        {
            var listOfClasses = _GetClassValue(element).Split(SPACE);
            var firstInstance = listOfClasses.FirstOrDefault(x => x.StartsWith(startingNeedle));

            return firstInstance ?? "";
        }

        public float ParseFirstFloatFromString(string text)
        {
            var matches = Regex.Matches(text, FLOAT_PATTERN);

            return float.Parse(matches[0].Value);
        }

        public int ParseFirstIntegerFromString(string text)
        {
            var matches = Regex.Matches(text, NUMBER_PATTERN);

            return int.Parse(matches[0].Value);
        }

        // From: https://stackoverflow.com/a/36002027
        public float ParseCurrencyWithSymbol(string text)
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .GroupBy(c => c.NumberFormat.CurrencySymbol)
                .ToDictionary(c => c.Key, c => c.First());


            var culture = cultures.FirstOrDefault(c => text.Contains(c.Key));

            float result = 0;
            if (!culture.Equals(default(KeyValuePair<string, CultureInfo>)))
            {
                result = float.Parse(text, NumberStyles.Currency);
            }
            else
            {
                if (!float.TryParse(text, out result))
                {
                    Log.Logger.Error("Invalid number format in ParserHelper.ParseCurrencyWithSymbol, text: {0}", text);
                    throw new Exception("Invalid number format");
                }
            }

            return result;
        }

        // Don't really care for this solution, but I'm tired of kicking CultureInfo code around to solve it.
        // PUNT in favor of my Culture for now :-/
        // EDIT: this may not be necessary - might have been blocked by my own bad unit
        // testing skills; other, better methods may now "work"
        public string RemoveCommasFromString(string text)
        {
            bool hasCommas = false;
            int index = text.IndexOf(',');

            hasCommas = index != -1;

            while (hasCommas)
            {
                text = text.Remove(index, 1);
                index = text.IndexOf(',');
                hasCommas = index != -1;
            }
            return text;
        }

        private string _GetClassValue(IElement element) {
            var classValue = element.GetAttribute(CLASS);
            return string.IsNullOrEmpty(classValue) ? "" : classValue;
        }
    }
}
