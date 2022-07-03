using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using XeniaPro.Localization.Core.Interfaces;
using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.Core.LocaleIItems;
using XeniaPro.Localization.Core.Models;

namespace XeniaPro.Localization.Core.LocaleTables;

public class NamespacedLocaleTable : INamespacedLocaleTable
{
    public Language Language { get; }

    public ReadOnlyCollection<string> Namespaces
        => _dictionary.Keys.ToList().AsReadOnly();
    
    private readonly Dictionary<string, List<ILocaleItem>> _dictionary;
    
    public NamespacedLocaleTable(Language language, Dictionary<string, List<ILocaleItem>> dictionary)
    {
        Language = language;
        _dictionary = dictionary;
    }
    
    public ILocaleItem GetItemByKey(string key)
    {
        var item = (from table in _dictionary.Values
            select table.Find(x => x.Key == key)).FirstOrDefault();
        return item ?? new InvalidLocaleItem(key);
    }

    public ILocaleItem GetNamespacedItemByKey(string @namespace, string key)
    {
        _dictionary.TryGetValue(@namespace, out var table);
        return table?.Find(x => x.Key == key) ?? new InvalidLocaleItem(key);
    }
}