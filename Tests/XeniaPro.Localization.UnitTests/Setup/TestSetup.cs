using System.Text.Json;
using XeniaPro.Localization.Files;
using XeniaPro.Localization.Web;

namespace XeniaPro.Localization.UnitTests.Setup;

public static class TestSetup
{
    private static WebLocalizationOptions? _webOptions;
    private static FileLocalizationOptions? _fileOptions;

    public static WebLocalizationOptions WebOptions
    {
        get
        {
            if (_webOptions is not null)
            {
                return _webOptions;
            }

            var json = File.ReadAllText($"{Directory.GetCurrentDirectory()}/web.json");
            var webLocalization = JsonSerializer.Deserialize<WebLocalizationOptions>(json);
            _webOptions = webLocalization;
            return WebOptions;
        }
    }
    
    public static FileLocalizationOptions FileOptions
    {
        get
        {
            if (_fileOptions is not null)
            {
                return _fileOptions;
            }

            var json = File.ReadAllText($"{Directory.GetCurrentDirectory()}/file.json");
            var fileOptions = JsonSerializer.Deserialize<FileLocalizationOptions>(json);
            _fileOptions = fileOptions;
            return FileOptions;
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