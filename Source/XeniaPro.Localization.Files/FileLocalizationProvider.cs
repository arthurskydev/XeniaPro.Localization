using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XeniaPro.Localization.Core.Exceptions;
using XeniaPro.Localization.Core.Interfaces;
using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.Core.LocaleTables;

namespace XeniaPro.Localization.Files;

public class FileLocalizationProvider : ILocalizationProvider
{
    private readonly string _resourcePath;
    private readonly ILogger<FileLocalizationProvider> _logger;
    private readonly ILanguageProvider _language;

    private readonly List<ILocaleTable> _tables;

    public FileLocalizationProvider(IOptions<FileLocalizationOptions> options, ILogger<FileLocalizationProvider> logger,
        ILanguageProvider language)
    {
        _logger = logger;
        _language = language;
        _resourcePath = options.Value.ResourcePath;

        _tables = Task.Run(LoadTables).Result;
    }

    private async Task<List<ILocaleTable>> LoadTables()
    {
        var tables = new List<ILocaleTable>();
        foreach (var language in _language.Languages)
        {
            var path = new Uri(_resourcePath + language.ShortHand + ".json");
            await using var fs = File.OpenRead(path.AbsolutePath);
            var strings = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(fs);
            tables.Add(new LocaleTable(strings, language));
        }

        return tables;
    }

    public ILocaleTable GetTable(Language language)
        => _tables.Find(x => x.Language == language) ?? throw new TableDoesNotExistException(language);
}