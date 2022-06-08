using System.Text.Json;
using XeniaPro.Localization.Models;

namespace XeniaPro.Localization.UnitTests.Setup;

public static class TestSetup
{
    private static RestLocalizationOptions? _options;
    private static string? _deJson;
    private static Dictionary<string, string>? _strings;
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

    public static string DeJson
    {
        get
        {
            if (_deJson is not null)
            {
                return _deJson;
            }
            _deJson = File.ReadAllText($"{Directory.GetCurrentDirectory()}/de.json");
            return DeJson;
        }
    }

    public static Dictionary<string, string> Strings
    {
        get
        {
            if (_strings is not null)
            {
                return _strings;
            }

            _strings = JsonSerializer.Deserialize<Dictionary<string, string>>(DeJson)!;
            return Strings;
        }
    }
}