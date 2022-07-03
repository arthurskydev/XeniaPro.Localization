using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XeniaPro.Localization.Core.Interfaces;
using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.Core.LocaleTables;
using XeniaPro.Localization.Core.Models;
using XeniaPro.Localization.Core.Options;

namespace XeniaPro.Localization.Web;

public enum TableStatus
{
    Loading,
    Loaded,
    Failed
}

public record LocaleTableContainer(TableStatus Status, Exception TableException, ILocaleTable Table);

public class WebLocalizationProvider : IAsyncLocalizationProvider
{
    private readonly HttpClient _client;
    private readonly ILogger<WebLocalizationProvider> _logger;
    private readonly object _lockObj = new();
    private readonly LocalizationOptions _options;


    private readonly List<LocaleTableContainer> _locales = new();

    public event Action LocalesUpdated;

    protected WebLocalizationProvider(HttpClient client, ILogger<WebLocalizationProvider> logger,
        IOptions<LocalizationOptions> locOptions)
    {
        _client = client;
        _logger = logger;
        _options = locOptions.Value;
    }

    public WebLocalizationProvider(IOptions<WebLocalizationOptions> options, ILogger<WebLocalizationProvider> logger,
        IOptions<LocalizationOptions> locOptions)
    {
        _logger = logger;
        _client = new HttpClient();
        _options = locOptions.Value;
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
            LocaleTableContainer containerObj;
            if (_options.NamespacesEnabled)
                containerObj = await _client.LoadNamespacedTable(language);
            else
                containerObj = await _client.LoadStandardTable(language);
            
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