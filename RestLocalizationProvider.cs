using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace XeniaPro.Localization;

public class RestLocalizationProvider : ILocalizationProvider
{
    private readonly List<ILocaleTable> _locales = new();
    private readonly RestLocalizationOptions _options;
    private readonly HttpClient _client;
    
    public event Action LanguagesUpdated;

    public RestLocalizationProvider(RestLocalizationOptions options)
    {
        _options = options;
        _client = new HttpClient();
        _client.BaseAddress = new Uri(options.HostUri);
    }

    public ILocaleTable GetTableFor(Language language)
    {
        var table = _locales.Find(x => x.Language == language);
        if (table is not null)
        {
            return table;
        }

        Task.Run(() => LoadTable(language));
        _locales.Add(new LocaleTable(new Dictionary<string, string>(), language));
        return GetTableFor(language);
    }

    private async Task LoadTable(Language language)
    {
        var httpResult = await _client.GetFromJsonAsync<Dictionary<string, string>>($"{language.ShortHand}.json");
        if (httpResult is null) throw new TableDoesNotExistException(language);
        var table = _locales.First(x => x.Language == language);
        table = new LocaleTable(httpResult, table.Language);
        LanguagesUpdated?.Invoke();
    }
}