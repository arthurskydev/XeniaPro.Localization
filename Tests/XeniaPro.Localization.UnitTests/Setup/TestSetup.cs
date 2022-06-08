using System.Text.Json;
using XeniaPro.Localization.Models;

namespace XeniaPro.Localization.UnitTests.Setup;

public static class TestSetup
{
    private static RestLocalizationOptions? _options;

    public static RestLocalizationOptions RestOptions
    {
        get
        {
            if (_options is not null)
            {
                return _options;
            }

            var restOptionsJson = File.ReadAllText($"{Directory.GetCurrentDirectory()}/restOptions.json");
            var restOptions = JsonSerializer.Deserialize<RestLocalizationOptions>(restOptionsJson);
            _options = restOptions;
            return RestOptions;
        }
    }

    private static readonly Dictionary<string, string> Locales = new();
    public static string GetLocaleFile(string langCode)
    {
        if (Locales.ContainsKey(langCode))
        {
            return Locales[langCode];
        }
        
        var json = File.ReadAllText($"{Directory.GetCurrentDirectory()}/{langCode}.json");
        Locales.Add(langCode, json);
        return json;
    }

    private static readonly Dictionary<string, Dictionary<string, string>> Dictionaries = new();
    public static Dictionary<string, string> GetDictionary(string langCode)
    {
            if (Dictionaries.ContainsKey(langCode))
            {
                return Dictionaries[langCode];
            }

            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(GetLocaleFile(langCode));
            Dictionaries.Add(langCode, dict!);
            return dict!;
    }
}