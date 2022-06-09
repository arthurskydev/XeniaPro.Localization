using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XeniaPro.Localization.Abstractions;
using XeniaPro.Localization.Abstractions.Exceptions;

namespace XeniaPro.Localization.Core.LanguageProviders;

public class LanguageProvider : ILanguageProvider
{
    public event Action LanguageUpdated;

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
    private readonly ILogger<LanguageProvider> _logger; 

    public LanguageProvider(IOptions<ILocalizationOptions> options, ILogger<LanguageProvider> logger)
    {
        _logger = logger;
        _languages = options.Value.Languages;
        _logger.LogInformation("Languages initialized");
        _logger.LogInformation("{Count} languages, {Languages}", _languages.Count, _languages);
    }

    public ICollection<Language> Languages
        => _languages.AsReadOnly();
    
    public void SetLanguage(Language language)
    {
        _logger.LogInformation("Setting language to {Language}", language);
        if (!_languages.Contains(language)) 
            throw new InvalidLanguageException(language);
        LanguageUpdated?.Invoke();
        _currentLanguage = language;
    }
}