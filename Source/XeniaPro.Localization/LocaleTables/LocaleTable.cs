using System.Collections.Generic;
using XeniaPro.Localization.Models;

namespace XeniaPro.Localization.LocaleTables;

public class LocaleTable : ILocaleTable
{
    private Dictionary<string, string> StringCollection { get; }
    public Language Language { get; }

    public LocaleTable(Dictionary<string, string> stringCollection, Language language)
    {
        StringCollection = stringCollection;
        Language = language;
    }

    public string GetByKey(string key)
        => StringCollection.TryGetValue(key, out var result) ? result : string.Empty;

    public static LocaleTable CreateEmpty(Language language)
        => new LocaleTable(new Dictionary<string, string>(), language);
}