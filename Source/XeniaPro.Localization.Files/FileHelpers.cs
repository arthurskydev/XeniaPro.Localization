using System.Text.Json;
using XeniaPro.Localization.Core.Exceptions;
using XeniaPro.Localization.Core.Interfaces;
using XeniaPro.Localization.Core.LocaleIItems;
using XeniaPro.Localization.Core.LocaleTables;
using XeniaPro.Localization.Core.Models;

namespace XeniaPro.Localization.Files;

public static class FileHelpers
{
    public static async Task<ILocaleTable> LoadTableAsync(Language language, string resPath)
    {
        var path = new Uri(resPath + $"/{language.LocaleName}.json");
        
        await using var fs = File.OpenRead(path.AbsolutePath);
        var strings = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(fs);
        return new LocaleTable(strings, language);
    }

    public static async Task<ILocaleTable> LoadNamespacedTableAsync(Language language, string resPath)
    {
        var path = new Uri(resPath + $"{language.LocaleName}");
        if (!File.Exists(path.AbsolutePath + "/loc.index"))
            throw new IndexNotFoundException(language);
        var index = await File.ReadAllLinesAsync(path.AbsolutePath + "/loc.index");
        
        Dictionary<string, List<ILocaleItem>> dictionary = new();
        foreach (var line in index)
        {
            await using var fs = File.OpenRead(path.AbsolutePath + $"/{line}.json");
            var strings = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(fs);
            var list = strings?.Select(x => LocaleItemFactory.CreateFromKeyValue(x.Key, x.Value)).ToList();
            dictionary.Add(line, list);
        }

        return new NamespacedLocaleTable(language, dictionary);
    }
}