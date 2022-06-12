using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XeniaPro.Localization.Abstractions;
using XeniaPro.Localization.Core.Exceptions;
using XeniaPro.Localization.Core.LocaleTables;

namespace XeniaPro.Localization.Web;

public class WebLocalizationProvider : IAsyncLocalizationProvider
{
    private readonly HttpClient _client;
    private readonly ILogger<WebLocalizationProvider> _logger;
    private readonly object _lockObj = new();

    private enum TableStatus
    {
        Loading,
        Loaded,
        Failed
    }

    private record LocaleTableContainer(TableStatus Status, Exception TableException, ILocaleTable Table);

    private readonly List<LocaleTableContainer> _locales = new();

    public event Action LocalesUpdated;

    protected WebLocalizationProvider(HttpClient client, ILogger<WebLocalizationProvider> logger)
    {
        _client = client;
        _logger = logger;
    }

    public WebLocalizationProvider(IOptions<WebLocalizationOptions> options, ILogger<WebLocalizationProvider> logger)
    {
        _logger = logger;
        _client = new HttpClient();
        _client.BaseAddress = new Uri(options.Value.ResourceUrl);
    }

    public ILocaleTable GetTable(Language language)
    {
        LocaleTableContainer container;

        lock (_lockObj)
        {
            container = _locales.Find(x => x.Table.Language == language);
        }

        if (container is null)
        {
            container = new LocaleTableContainer(TableStatus.Loading, null, LocaleTable.CreateEmpty(language));
            lock (_lockObj)
            {
                _locales.Add(container);
            }

            Task.Run(() => LoadTable(language));
            return container.Table;
        }

        if (container.Status != TableStatus.Failed)
            return container.Table;

        throw container.TableException;
    }

    private async Task LoadTable(Language language)
    {
        try
        {
            var response = await _client.GetAsync($"{language.ShortHand}.json");
            if (!response.IsSuccessStatusCode)
                throw new TableDoesNotExistException(language);

            var strings =
                await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(
                    await response.Content.ReadAsStreamAsync());
            var containerObj = new LocaleTableContainer(TableStatus.Loaded, null, new LocaleTable(strings, language));
            lock (_lockObj)
            {
                var container = _locales.Find(x => x.Table.Language == language);
                _locales.Remove(container);
                _locales.Add(containerObj);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception was thrown while fetching {Language}", language);
            var containerObj = new LocaleTableContainer(TableStatus.Failed, ex, LocaleTable.CreateEmpty(language));
            lock (_lockObj)
            {
                var container = _locales.Find(x => x.Table.Language == language);
                _locales.Remove(container);
                _locales.Add(containerObj);
            }
        }
        finally
        {
            LocalesUpdated?.Invoke();
        }
    }
}