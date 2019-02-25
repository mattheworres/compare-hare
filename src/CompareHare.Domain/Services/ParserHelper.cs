using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using CompareHare.Domain.Services.Interfaces;

namespace CompareHare.Domain.Services
{
    public class ParserHelper : IParserHelper
    {
        private const string SPACE = " ";
        private const string CLASS = "class";
        private const string NUMBER_PATTERN = @"^\d+";

        public bool ElementHasClass(IElement element, string className, bool caseSensitive = true)
        {
            var listOfClasses = element.GetAttribute(CLASS).Split(SPACE);

            if (caseSensitive) return listOfClasses.Any(x => x == className);

            return listOfClasses.Any(x => x.ToLowerInvariant() == className);
        }

        public IEnumerable<string> GetClasses(IElement element)
        {
            return element.GetAttribute("class").Split(SPACE);
        }

        public string GetElementClassStartingWith(IElement element, string startingNeedle)
        {
            var listOfClasses = element.GetAttribute(CLASS).Split(SPACE);

            return listOfClasses.FirstOrDefault(x => x == startingNeedle);
        }

        public float ParseFirstFloatFromString(string text)
        {
            var matches = Regex.Matches(text, NUMBER_PATTERN);

            return float.Parse(matches[0].Value);
        }

        public int ParseFirstIntegerFromString(string text)
        {
            var matches = Regex.Matches(text, NUMBER_PATTERN);

            return int.Parse(matches[0].Value);
        }


    }
}
