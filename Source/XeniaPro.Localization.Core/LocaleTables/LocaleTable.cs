using System.Collections.Generic;
using System.Linq;
using XeniaPro.Localization.Core.Interfaces;
using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.Core.LocaleIItems;
using XeniaPro.Localization.Core.Models;

namespace XeniaPro.Localization.Core.LocaleTables;

public class LocaleTable : ILocaleTable
{
    private List<ILocaleItem> StringCollection { get; }
    public Language Language { get; }

    public LocaleTable(Dictionary<string, string> stringCollection, Language language)
    {
        StringCollection = (from item in stringCollection
                            select LocaleItemFactory.CreateFromKeyValue(item.Key, item.Value)).ToList();
        Language = language;
    }

    public ILocaleItem GetItemByKey(string key)
        => StringCollection.Find(x => x.Key == key) ?? new InvalidLocaleItem(key);

    public static LocaleTable CreateEmpty(Language language) => new(new Dictionary<string, string>(), language);
}