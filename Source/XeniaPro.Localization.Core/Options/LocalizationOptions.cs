using System.Collections.Generic;
using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.Core.Models;

namespace XeniaPro.Localization.Core.Options;

public class LocalizationOptions
{
    /// <summary>
    /// Set a string that is passed if no localized string can be matched to a given key. Set this value to "." if you want the key to be passed through.
    /// </summary>
    public string PlaceholderString { get; set; } = ".";
    
    /// <summary>
    /// Provide a list of languages that are know to be available.
    /// </summary>
    public List<Language> Languages { get; set; } = new();
}