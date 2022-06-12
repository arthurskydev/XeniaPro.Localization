using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeniaPro.Localization.Abstractions;
using XeniaPro.Localization.Core.Exceptions;

namespace XeniaPro.Localization.Core.LocaleIItems;

public static class LocaleItemFactory
{
    public static ILocaleItem CreateFromKeyValue(string key, string value)
    {
        var unescapedBegins = value.GetUnescaped('{');
        var unescapedTerms = value.GetUnescaped('}');


        if (!unescapedBegins.Any() || !unescapedTerms.Any())
        {
            return new PlainLocaleItem(key, value.CleanEscapedString('{', '}', '@'));
        }

        if (unescapedBegins.Count != unescapedTerms.Count)
            throw new InvalidStringInterpolationException(key);


        var beginsDeNested = unescapedBegins
            .Where((_, i) => i == 0 || unescapedBegins[i - 1] > unescapedTerms[i]).Select(x => x).ToList();
        var termsDeNested = unescapedTerms
            .Where((_, i) => i == unescapedTerms.Count-1 || unescapedTerms[i + 1] < unescapedBegins[i]).Select(x => x).ToList();


        var zipped = beginsDeNested.Zip(termsDeNested).ToList();

        var valLen = value.Length;
        var valueBuilder = new StringBuilder(value);
        var subItems = zipped.Select(item =>
        {
            var valDiff = valLen - value.Length;
            var substr = value.Substring(item.First, item.Second - item.First);
            var split = substr.Split(':', 2);
            if (split.Length != 2)
                throw new InvalidStringInterpolationException(key);
            
            var result = CreateFromKeyValue(split[0][1..], split[1]);
            valueBuilder.Remove(item.First - valDiff, item.Second - item.First);
            valueBuilder.Insert(item.First - valDiff, result.GetString());
            return result;
        }).ToList();

        var lastIdx = 0;
        List<string> skeleton = new();
        foreach (var item in zipped)
        {
            skeleton.Add(value.Substring(lastIdx, item.First - lastIdx).CleanEscapedString('@', '{', '}'));
            lastIdx = item.Second;
        }

        skeleton.Add(value.Substring(lastIdx, value.Length - lastIdx).CleanEscapedString('@', '{', '}'));


        return new ComplexLocaleItem(key, valueBuilder.ToString().CleanEscapedString('{', '}', '@'), skeleton, subItems);
    }

    private static List<int> GetUnescaped(this string value, char target)
    {
        var indices = new List<int>();
        for (var i = 0; i < value.Length; i++)
        {
            if (value[i] != target) continue;
            if (i > 0 && value.ElementAt(i - 1) == target) continue;
            if (i + 1 < value.Length && value.ElementAt(i + 1) == target) continue;

            indices.Add(i);
        }

        return indices;
    }

    private static string CleanEscapedString(this string value, params char[] escapeChars)
    {
        var sb = new StringBuilder(value);
        List<int> purge = new();

        var escapeList = escapeChars.ToList();
        for (var i = 0; i < value.Length; i++)
        {
            if (!escapeList.Contains(value[i])) continue;
            var target = value[i];
            if (i > 0 && value[i-1] == target) continue;
            purge.Add(i);
        }

        for (var j = 0; j < purge.Count; j++)
        {
            sb.Remove(purge[j] - j, 1);
        }
        
        return sb.ToString();
    }
}