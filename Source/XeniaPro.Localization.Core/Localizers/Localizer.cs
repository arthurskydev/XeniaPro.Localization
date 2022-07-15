using Microsoft.Extensions.Options;
using XeniaPro.Localization.Core.Interfaces;
using XeniaPro.Localization.Core.Options;

namespace XeniaPro.Localization.Core.Localizers;

public class Localizer : ILocalizer
{
    private readonly ILocalizationProvider _provider;
    private readonly ILanguageProvider _language;
    private readonly bool _keyOnEmpty;
    private readonly string _placeholder;

    public Localizer(ILocalizationProvider provider, ILanguageProvider language,
        IOptions<LocalizationOptions> options)
    {
        _provider = provider;
        _language = language;
        _keyOnEmpty = options.Value.PlaceholderString == ".";
        _placeholder = options.Value.PlaceholderString;
    }

    public string Get(string key)
        => ReturnString(key);

    public string Get(string key, params string[] args)
        => ReturnString(key, null, args);

    public string Get(string key, string @namespace)
        => ReturnString(key, @namespace);

    public string Get(string key, string @namespace, params string[] args)
        => ReturnString(key, @namespace, args);

    private string ReturnString(string key, string @namespace = null, string[] args = null)
    {
        var result = _provider.GetTable(_language.CurrentLanguage, @namespace)
            .GetItemByKey(key)
            .GetString(args);
        if (string.IsNullOrEmpty(result))
            return _keyOnEmpty ? key : _placeholder;

        return result;
    }
}