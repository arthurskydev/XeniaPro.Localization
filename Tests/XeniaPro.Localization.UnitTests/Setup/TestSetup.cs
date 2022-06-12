using System.Text.Json;
using XeniaPro.Localization.Core;
using XeniaPro.Localization.Files;
using XeniaPro.Localization.Web;

namespace XeniaPro.Localization.UnitTests.Setup;

public static class TestSetup
{
    private static LocalizationOptions? _options;

    public static LocalizationOptions Options
    {
        get
        {
            if (_options is not null)
            {
                return _options;
            }

            var json = File.ReadAllText($"{Directory.GetCurrentDirectory()}/web.json");
            var options = JsonSerializer.Deserialize<LocalizationOptions>(json);
            _options = options;
            return Options;
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