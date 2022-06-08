using XeniaPro.Localization.LanguageProviders;
using XeniaPro.Localization.LocaleProviders;

namespace XeniaPro.Localization.Localizers;

public class AsyncLocalizer : ILocalizer
{
    private readonly IAsyncLocalizationProvider _provider;
    private readonly ILanguageProvider _language;

    public AsyncLocalizer(IAsyncLocalizationProvider provider, ILanguageProvider language)
    {
        _provider = provider;
        _language = language;
    }

    public string this[string key] => Get(key);

    public string Get(string key)
    {
        var task = _provider.GetTableAsync(_language.CurrentLanguage);
        return task.IsCompleted ? task.Result.GetByKey(key) : string.Empty;
    }
}