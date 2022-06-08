using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using XeniaPro.Localization.Exceptions;
using XeniaPro.Localization.LocaleTables;
using XeniaPro.Localization.Models;

namespace XeniaPro.Localization.LocaleProviders;

public class RestLocalizationProvider : IAsyncLocalizationProvider
{
    private readonly List<ILocaleTable> _locales = new();
    private readonly HttpClient _client;

    public event Action LanguagesUpdated;

    public RestLocalizationProvider(RestLocalizationOptions options)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(options.HostUri);
    }

    public RestLocalizationProvider(HttpClient client)
    {
        _client = client;
    }

    public RestLocalizationProvider(IOptions<RestLocalizationOptions> options)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(options.Value.HostUri);
    }

    public ILocaleTable GetTableFor(Language language)
    {
        var table = _locales.Find(x => x.Language == language);
        if (table is not null)
        {
            return table;
        }

        LoadTable(language).ConfigureAwait(false);
        _locales.Add(new LocaleTable(new Dictionary<string, string>(), language));
        return GetTableFor(language);
    }

    private async Task LoadTable(Language language)
    {
        await using var httpStream = await _client.GetStreamAsync($"{language.ShortHand}.json");
        if (httpStream is null) throw new TableDoesNotExistException(language);
        var strings = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(httpStream);
        var table = new LocaleTable(strings, language);
        _locales.Remove(_locales.First(x => x.Language == language));
        _locales.Add(table);
        LanguagesUpdated?.Invoke();
    }
}