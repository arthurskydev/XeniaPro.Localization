using XeniaPro.Localization.Abstractions;

namespace XeniaPro.Localization.Core.Localizers;

public class Localizer : ILocalizer
{
    private readonly ILocalizationProvider _provider;
    private readonly ILanguageProvider _language;

    public Localizer(ILocalizationProvider provider, ILanguageProvider language)
    {
        _provider = provider;
        _language = language;
    }

    public string this[string key] => Get(key);

    public string Get(string key) 
        => _provider.GetTable(_language.CurrentLanguage).GetByKey(key);
}