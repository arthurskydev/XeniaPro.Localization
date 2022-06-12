using System.Collections.Generic;
using System.Linq;
using XeniaPro.Localization.Abstractions;

namespace XeniaPro.Localization.Core.LocaleTables;

public class LocaleTable : ILocaleTable
{
    private List<ILocaleItem> StringCollection { get; }
    public Language Language { get; }

    public LocaleTable(Dictionary<string, string> stringCollection, Language language)
    {
        StringCollection = (from item in stringCollection
                            select LocaleItemFactory.FromKeyValuePair(item)).ToList();
        Language = language;
    }

    public ILocaleItem GetItemByKey(string key)
        => StringCollection.Find(x => x.Key == key) ?? new InvalidLocaleItem(key);

    public static LocaleTable CreateEmpty(Language language) => new(new Dictionary<string, string>(), language);
}