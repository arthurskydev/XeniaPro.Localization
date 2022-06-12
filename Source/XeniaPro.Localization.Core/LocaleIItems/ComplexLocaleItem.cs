using System.Collections.Generic;
using System.Linq;
using XeniaPro.Localization.Abstractions;

namespace XeniaPro.Localization.Core.LocaleIItems;

public readonly struct ComplexLocaleItem : ILocaleItem
{
    public string Key { get; }
    public string Value { get; }
    private List<string> Skeleton { get; }
    private List<ILocaleItem> SubItems { get; }
    
    public ComplexLocaleItem(string key, string value, List<string> skeleton, List<ILocaleItem> subItems)
    {
        Key = key;
        Value = value;
        Skeleton = skeleton;
        SubItems = subItems;
    }
    
    public string GetString()
    {
        return Value;
    }

    public string GetString(params string[] args)
    {
        var parsed = int.TryParse(args.First(), out var idx);
        if (parsed)
            return Skeleton.ElementAtOrDefault(idx);

        var found = SubItems.Find(x => x.Key == args.First());
        return found?.GetString(args[1..]) ?? string.Empty;
    }

}
