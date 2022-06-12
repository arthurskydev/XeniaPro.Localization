using System.Linq;
using Microsoft.Extensions.Options;
using XeniaPro.Localization.Abstractions;
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

    public string this[string key] => Get(key);

    public string Get(string key)
    {
        var result = _provider.GetTable(_language.CurrentLanguage).GetItemByKey(key).GetString();
        return ReturnString(key, result);
    }

    public string Get(string key, params string[] args)
    {
        var result = _provider.GetTable(_language.CurrentLanguage).GetItemByKey(key).GetString(args.First());
        return ReturnString(key, result);
    }

    private string ReturnString(string key, string value)
    {
        if (!string.IsNullOrEmpty(value))
            return value;

        return _keyOnEmpty ? key : _placeholder;
    }
}