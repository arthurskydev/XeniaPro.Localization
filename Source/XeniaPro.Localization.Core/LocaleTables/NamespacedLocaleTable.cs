using System.Collections.Generic;
using XeniaPro.Localization.Core.Interfaces;
using XeniaPro.Localization.Core.Models;

namespace XeniaPro.Localization.Core.LocaleTables;

public class NamespacedLocaleTable : LocaleTable, INamespacedLocaleTable
{
    public string Namespace { get; }

    public NamespacedLocaleTable(Language language, string @namespace, Dictionary<string, string> localeStrings) : base(localeStrings, language)
    {
        Namespace = @namespace;
    }

    public static NamespacedLocaleTable CreateEmpty(Language language, string @namespace) => new NamespacedLocaleTable(language, @namespace, new Dictionary<string, string>());
}