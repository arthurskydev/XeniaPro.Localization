using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XeniaPro.Localization.Core.Interfaces;
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

public interface ILocaleTableContainer
{
    ILocaleTable Table { get; }
    TableStatus Status { get; }
    Exception TableException { get; }
}

public record LocaleTableContainer(TableStatus Status, Exception TableException, ILocaleTable Table): ILocaleTableContainer;
public record NamespacedLocaleTableContainer(TableStatus Status, Exception TableException, INamespacedLocaleTable Table) : ILocaleTableContainer
{
    ILocaleTable ILocaleTableContainer.Table => Table;
}

public class WebLocalizationProvider : ILocalizationProvider
{
    private readonly HttpClient _client;
    private readonly ILogger<WebLocalizationProvider> _logger;
    private readonly object _lockObj = new();
    private readonly LocalizationOptions _options;


    private readonly List<ILocaleTableContainer> _locales = new();

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

    public async Task<ILocaleTable> GetTableAsync(Language language)
    {
        LocaleTableContainer container;

        lock (_lockObj)
        {
            var query = from item in _locales
                        where item is LocaleTableContainer && (item as LocaleTableContainer).Table.Language == language
                        select item as LocaleTableContainer;

            container = query.FirstOrDefault();
        }

        if (container is null)
        {
            container = new LocaleTableContainer(TableStatus.Loading, null, LocaleTable.CreateEmpty(language));

            lock (_lockObj)
            {
                _locales.Add(container);
            }

            await LoadTable(language);
            return container.Table;
        }

        if (container.Status != TableStatus.Failed)
            return container.Table;

        throw container.TableException;
    }

    public async Task<INamespacedLocaleTable> GetTableAsync(Language language, string @namespace)
    {
        NamespacedLocaleTableContainer container;

        lock (_lockObj)
        {
            var query = from item in _locales
                        where item is NamespacedLocaleTable 
                        && (item as NamespacedLocaleTableContainer).Table.Language == language 
                        && (item as NamespacedLocaleTableContainer).Table.Namespace == @namespace
                        select item as NamespacedLocaleTableContainer;
            container = query.FirstOrDefault();
        }

        if (container is null)
        {
            container = new NamespacedLocaleTableContainer(TableStatus.Loading, null, NamespacedLocaleTable.CreateEmpty(language, @namespace));

            lock (_lockObj)
            {
                _locales.Add(container);
            }

            await LoadTable(language, @namespace);
            return container.Table as NamespacedLocaleTable;
        }

        if (container.Status != TableStatus.Failed)
            return container.Table as NamespacedLocaleTable;

        throw container.TableException;
    }

    private async Task LoadTable(Language language, string @namespace = null)
    {
        try
        {
            ILocaleTableContainer containerObj;
            if (!string.IsNullOrEmpty(@namespace))
                containerObj = await _client.LoadStandardTable(language);
            else
                containerObj = await _client.LoadNamespacedTable(language, @namespace);

            ILocaleTableContainer currentContainer;
            lock (_lockObj)
            {
                if (string.IsNullOrEmpty(@namespace))
                {
                    currentContainer = _locales.Find(x => x.Table.Language == language);
                }
                else
                {
                    currentContainer = _locales
                        .Find(x => x is NamespacedLocaleTableContainer 
                        && x.Table.Language == language 
                        && (x as NamespacedLocaleTableContainer).Table.Namespace == @namespace);
                }
                _locales.Remove(currentContainer);
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
            LocalesUpdated.Invoke();
        }
    }
}