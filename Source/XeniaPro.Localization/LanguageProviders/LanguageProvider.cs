using System.Collections.Generic;
using System.Linq;
using XeniaPro.Localization.Exceptions;
using XeniaPro.Localization.Models;

namespace XeniaPro.Localization.LanguageProviders;

public class LanguageProvider : ILanguageProvider
{
    public Language CurrentLanguage { get; private set; }

    private readonly List<Language> _languages;

    public LanguageProvider(RestLocalizationOptions options)
    {
        _languages = options.Languages;
        CurrentLanguage = _languages.First();
    }

    public ICollection<Language> GetLanguages
        => _languages.AsReadOnly();
    
    public void SetLanguage(Language language)
    {
        if (!_languages.Contains(language)) 
            throw new InvalidLanguageException(language);
        CurrentLanguage = language;
    }
}