using System.Collections.Generic;
using System.Text;
using XeniaPro.Localization.Abstractions;

namespace XeniaPro.Localization.Core.LocaleTables;

public static class LocaleItemFactory
{
    public static ILocaleItem FromKeyValuePair(KeyValuePair<string, string> localeItem)
    {
        var value = localeItem.Value;
        if (!value.Contains("@{"))
            return new PlainLocaleItem(localeItem.Key, value);

        var items = new Dictionary<string, string>();
        var skeleton = new List<string>();
        
        List<int> targets = new();
        for (var i = 0; i < value.Length; i++)
        {
            if (i + 2 < value.Length && value[i] != '@' && value[i + 1] != '{')
                continue;
            if (i > 1 && value[i - 1] == '@')
                continue;
            targets.Add(i);
        }

        var prevIdx = 0;
        foreach (var target in targets)
        {
            var len = 0;
            var lastIdx = 0;
            for (var i = target + 2; i < value.Length; i++)
            {
                if (value[i] != '}') continue;
                len = target + 2 - i;
                lastIdx = i;
            }

            var item = value.Substring(target + 2, len).Split(';');
            var skeletonPreceding = value.Substring(prevIdx, target - prevIdx);
            var sb = new StringBuilder();
            sb.AppendJoin(';', item[1..]);
            items.Add(item[0], sb.ToString());
            skeleton.Add(skeletonPreceding);
            prevIdx = lastIdx;
        }

        return new ComplexLocaleItem(localeItem.Key, skeleton.ToArray(), items);
    }
}