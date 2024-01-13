using System.Globalization;

using System.Text.RegularExpressions;
using System.Threading;

namespace CompareHare.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string SpaceBeforeCapitalLetters(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var words = Regex.Matches(str, @"([A-Z][a-z]+)")
                             .Select(m => m.Value);

            return string.Join(" ", words);
        }

        public static string ToTitleCase(this string str, CultureInfo? cultureInfo = null)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            if (cultureInfo == null)
                cultureInfo = Thread.CurrentThread.CurrentCulture;

            var titleCaseStr = cultureInfo.TextInfo.ToTitleCase(str.ToLower());

            titleCaseStr = titleCaseStr.Replace(" A ", " a ");
            titleCaseStr = titleCaseStr.Replace(" An ", " an ");
            titleCaseStr = titleCaseStr.Replace(" And ", " and ");
            titleCaseStr = titleCaseStr.Replace(" As ", " as ");
            titleCaseStr = titleCaseStr.Replace(" At ", " at ");
            titleCaseStr = titleCaseStr.Replace(" But ", " But ");
            titleCaseStr = titleCaseStr.Replace(" By ", " by ");
            titleCaseStr = titleCaseStr.Replace(" For ", " for ");
            titleCaseStr = titleCaseStr.Replace(" From ", " from ");
            titleCaseStr = titleCaseStr.Replace(" In ", " in ");
            titleCaseStr = titleCaseStr.Replace(" Into ", " into ");
            titleCaseStr = titleCaseStr.Replace(" Has ", " has ");
            titleCaseStr = titleCaseStr.Replace(" Had ", " had ");
            titleCaseStr = titleCaseStr.Replace(" Is ", " is ");
            titleCaseStr = titleCaseStr.Replace(" Nor ", " nor ");
            titleCaseStr = titleCaseStr.Replace(" Not ", " not ");
            titleCaseStr = titleCaseStr.Replace(" Of ", " of ");
            titleCaseStr = titleCaseStr.Replace(" Off ", " off ");
            titleCaseStr = titleCaseStr.Replace(" On ", " on ");
            titleCaseStr = titleCaseStr.Replace(" Onto ", " onto ");
            titleCaseStr = titleCaseStr.Replace(" Or ", " or ");
            titleCaseStr = titleCaseStr.Replace(" To ", " to ");
            titleCaseStr = titleCaseStr.Replace(" That ", " that ");
            titleCaseStr = titleCaseStr.Replace(" This ", " this ");
            titleCaseStr = titleCaseStr.Replace(" Thus ", " thus ");
            titleCaseStr = titleCaseStr.Replace(" The ", " the ");
            titleCaseStr = titleCaseStr.Replace(" Too ", " too ");
            titleCaseStr = titleCaseStr.Replace(" With ", " with ");
            titleCaseStr = titleCaseStr.Replace(" Yet ", " yet ");

            return titleCaseStr.FirstLetterToUpperCase();
        }

        public static string FirstLetterToUpperCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            var a = str.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}
