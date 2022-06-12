using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeniaPro.Localization.Abstractions;

namespace XeniaPro.Localization.Core.LocaleTables;

public struct ComplexLocaleItem : ILocaleItem
{
    public string Key { get; }

    private string _value = null;

    private string Value
    {
        get
        {
            if (_value is not null)
                return _value;
            var sb = new StringBuilder();
            for (var i = 0; i < Skeleton.Length; i++)
            {
                sb.Append(Skeleton[i]);
                sb.Append(Items.ElementAt(i));
            }

            _value = sb.ToString();
            return _value;
        }
    }

    private string[] Skeleton { get; }
    private Dictionary<string, string> Items { get; }
    
    public ComplexLocaleItem(string key, string[] skeleton, Dictionary<string, string> items)
    {
        Key = key;
        Skeleton = skeleton;
        Items = items;
    }
    
    public string GetString()
    {
        return Value;
    }

    public string GetString(string secondaryKey)
    {
        var parsed = int.TryParse(secondaryKey, out var idx);
        if (parsed)
            return Skeleton.ElementAtOrDefault(idx);

        var found = Items.TryGetValue(secondaryKey!, out var item);
        return found ? item : string.Empty;
    }

}
