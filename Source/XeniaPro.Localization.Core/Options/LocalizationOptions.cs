using System.Collections.Generic;
using XeniaPro.Localization.Core.LanguageProviders;

namespace XeniaPro.Localization.Core.Options;

public class LocalizationOptions
{
    public string PlaceholderString { get; set; }
    public List<Language> Languages { get; set; }
}