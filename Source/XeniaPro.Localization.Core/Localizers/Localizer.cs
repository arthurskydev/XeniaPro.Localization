using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XeniaPro.Localization.Core.Interfaces;
using XeniaPro.Localization.Core.Options;

namespace XeniaPro.Localization.Core.Localizers;

public class Localizer : ILocalizer
{
    private readonly ILocalizationProvider _provider;
    private readonly ILanguageProvider _language;
    private readonly ILogger<Localizer> _logger;
    private readonly bool _keyOnEmpty;
    private readonly string _placeholder;

    public Localizer(ILocalizationProvider provider, ILanguageProvider language,
        IOptions<LocalizationOptions> options, ILogger<Localizer> logger)
    {
        _provider = provider;
        _language = language;
        _logger = logger;
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

        _logger.LogWarning("String for {Key} was not found", key);
        return _keyOnEmpty ? key : _placeholder;
    }
}