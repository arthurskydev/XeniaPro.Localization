using System.Collections.Generic;

namespace XeniaPro.Localization;

public class LanguageProvider : ILanguageProvider
{
    public Language CurrentLanguage { get; private set; }

    private readonly List<Language> _languages;

    public LanguageProvider(RestLocalizationOptions options)
    {
        _languages = options.Languages;
    }

    public ICollection<Language> GetLanguages
        => _languages.AsReadOnly();
    
    public void SetLanguage(Language language)
    {
        CurrentLanguage = language;
    }
}