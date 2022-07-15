using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XeniaPro.Localization.Core.Exceptions;
using XeniaPro.Localization.Core.Interfaces;
using XeniaPro.Localization.Core.Models;
using XeniaPro.Localization.Core.Options;

namespace XeniaPro.Localization.Files;

public class FileLocalizationProvider : ILocalizationProvider
{
    private readonly string _resourcePath;
    private readonly ILogger<FileLocalizationProvider> _logger;
    private readonly ILanguageProvider _language;
    private readonly LocalizationOptions _options;

    private readonly List<ILocaleTable> _tables;

    public event Action LocalesUpdated;

    public FileLocalizationProvider(IOptions<FileLocalizationOptions> options, ILogger<FileLocalizationProvider> logger,
        ILanguageProvider language, IOptions<LocalizationOptions> locOptions)
    {
        _logger = logger;
        _language = language;
        _resourcePath = options.Value.ResourcePath;
        _options = locOptions.Value;
        _tables = Task.Run(LoadTables).Result;
    }

    private async Task<List<ILocaleTable>> LoadTables()
    {
        _logger.LogInformation("Loading locales from files");
        var tables = new List<ILocaleTable>();

        foreach (var language in _language.Languages)
        {
            ILocaleTable table;
            if (!_options.NamespacesEnabled)
                table = await FileHelpers.LoadTableAsync(language, _resourcePath);
            else
                table = await FileHelpers.LoadNamespacedTableAsync(language, _resourcePath);
            tables.Add(table);
            _logger.LogInformation("Loaded table for {Lang}", language);
        }

        return tables;
    }

    public ILocaleTable GetTable(Language language)
        => _tables.Find(x => x.Language == language) ?? throw new TableDoesNotExistException(language);
}