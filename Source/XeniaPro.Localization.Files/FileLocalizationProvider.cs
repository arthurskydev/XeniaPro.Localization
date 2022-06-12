using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XeniaPro.Localization.Abstractions;
using XeniaPro.Localization.Core.Exceptions;
using XeniaPro.Localization.Core.LocaleTables;

namespace XeniaPro.Localization.Files;

public class FileLocalizationProvider : IAsyncLocalizationProvider
{
    private readonly string _resourcePath;
    private readonly ILogger<FileLocalizationProvider> _logger;
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

    public FileLocalizationProvider(IOptions<FileLocalizationOptions> options, ILogger<FileLocalizationProvider> logger)
    {
        _logger = logger;
        _resourcePath = options.Value.ResourcePath;
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
            var path = new Uri(Directory.GetCurrentDirectory() + _resourcePath + $"{language.ShortHand}.json");
            if (!File.Exists(path.AbsolutePath))
            {
                throw new TableDoesNotExistException(language);
            }
            
            await using var fs =  File.OpenRead(path.AbsolutePath);
            var strings = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(fs);
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