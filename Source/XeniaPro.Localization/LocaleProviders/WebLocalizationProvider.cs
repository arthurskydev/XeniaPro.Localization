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

public class WebLocalizationProvider : IAsyncLocalizationProvider
{
    private readonly List<ILocaleTable> _locales = new();
    private readonly HttpClient _client;
    private readonly object _lockObj = new();

    protected WebLocalizationProvider(HttpClient client)
    {
        _client = client;
    }

    public WebLocalizationProvider(IOptions<WebLocalizationOptions> options)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(options.Value.ResourceUrl);
    }

    public event Action LanguagesUpdated;

    public async ValueTask<ILocaleTable> GetTableAsync(Language language)
    {
        ILocaleTable table;
        lock (_lockObj)
        {
            table = _locales.Find(x => x.Language == language);
        }
        
        if (table is not null)
        {
            return table;
        }

        lock (_lockObj)
        {
            _locales.Add(LocaleTable.CreateEmpty(language));
        }
        
        var httpResult = await _client.GetAsync($"{language.ShortHand}.json");
        
        if (!httpResult.IsSuccessStatusCode)
        {
            throw new TableDoesNotExistException(language);
        }

        var strings =
            await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(
                await httpResult.Content.ReadAsStreamAsync());
        
        table = new LocaleTable(strings, language);

        lock (_lockObj)
        {
            _locales.Remove(_locales.First(x => x.Language == language));
            _locales.Add(table);
        }
        
        LanguagesUpdated?.Invoke();
        return table;
    }
}