using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using XeniaPro.Localization.Exceptions;
using XeniaPro.Localization.Models;

namespace XeniaPro.Localization.LanguageProviders;

public class LanguageProvider : ILanguageProvider
{
    public Language CurrentLanguage
    {
        get
        {
            if (_currentLanguage != null) return _currentLanguage;
            _currentLanguage = _languages.First();
            return CurrentLanguage;
        }
    }

    private Language _currentLanguage;
    private readonly List<Language> _languages;

    public LanguageProvider(IOptions<RestLocalizationOptions> options)
    {
        _languages = options.Value.Languages;
    }

    public ICollection<Language> Languages
        => _languages.AsReadOnly();
    
    public void SetLanguage(Language language)
    {
        if (!_languages.Contains(language)) 
            throw new InvalidLanguageException(language);
        _currentLanguage = language;
    }
}