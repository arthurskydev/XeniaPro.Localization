using System.Text.Json;
using XeniaPro.Localization.Core.Exceptions;
using XeniaPro.Localization.Core.Interfaces;
using XeniaPro.Localization.Core.LocaleIItems;
using XeniaPro.Localization.Core.LocaleTables;
using XeniaPro.Localization.Core.Models;

namespace XeniaPro.Localization.Web;

public static class WebExtensions
{
    public static async Task<LocaleTableContainer> LoadNamespacedTable(this HttpClient client, Language language)
    {
        var response = await client.GetAsync($"{language.LocaleName}/loc.index");
        if (!response.IsSuccessStatusCode)
            throw new IndexNotFoundException(language);

        var index = await response.Content.ReadAsStringAsync();
        var indices = index.Split('\n');
        var dictionary = new Dictionary<string, List<ILocaleItem>>();
        foreach (var i in indices)
        {  
            var t = i.Trim();
            dictionary.Add(t, await client.LoadStrings(t, language));
        }

        var table = new NamespacedLocaleTable(language, dictionary);
        return new LocaleTableContainer(TableStatus.Loaded, null, table);
    }

    public static async Task<LocaleTableContainer> LoadStandardTable(this HttpClient client, Language language)
    {
        var response = await client.GetAsync($"{language.LocaleName}.json");
        if (!response.IsSuccessStatusCode)
            throw new TableDoesNotExistException(language);

        var strings =
            await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(
                await response.Content.ReadAsStreamAsync());
        return new LocaleTableContainer(TableStatus.Loaded, null, new LocaleTable(strings, language));
    }

    private static async Task<List<ILocaleItem>> LoadStrings(this HttpClient client, string @namespace, Language language)
    {
        var response = await client.GetAsync($"{language.LocaleName}/{@namespace}.json");
        if (!response.IsSuccessStatusCode)
            throw new TableDoesNotExistException(language, @namespace);
        var dict = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(await response.Content.ReadAsStreamAsync());
        return dict?.Select(x => LocaleItemFactory.CreateFromKeyValue(x.Key, x.Value)).ToList();
    }
}