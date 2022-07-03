using System.Globalization;
using System.Text.Json;
using RichardSzalay.MockHttp;
using XeniaPro.Localization.Core.Models;
using XeniaPro.Localization.Core.Options;

namespace XeniaPro.Localization.UnitTests.Setup;

public static class TestSetup
{
    public static LocalizationOptions Options => new()
    {
        PlaceholderString = ".",
        Languages = new List<Language>
        {
            new("Deutsch", "de", CultureInfo.InvariantCulture),
            new("English", "en", CultureInfo.InvariantCulture)
        }
    };

    public static string GetLocaleFile(string langCode)
    {
        var json = File.ReadAllText($"{Directory.GetCurrentDirectory()}/locales/{langCode}.json");
        return json;
    }
    
    public static string GetFile(string relPath)
    {
        var json = File.ReadAllText($"{Directory.GetCurrentDirectory()}/{relPath}");
        return json;
    }

    public static Dictionary<string, string> GetDictionary(string langCode)
    {
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(GetLocaleFile(langCode));
            return dict!;
    }
    
    public static HttpClient GetClient() {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When("http://localhost/locales/de.json")
            .Respond("application/json", GetLocaleFile("de"));

        mockHttp.When("http://localhost/locales/en.json")
            .Respond("application/json", GetLocaleFile("en"));

        mockHttp.When("http://localhost/locales/de/loc.index")
            .Respond("application/json", GetFile("locales/de/loc.index"));
        
        mockHttp.When("http://localhost/locales/en/loc.index")
            .Respond("application/json", GetFile("locales/en/loc.index"));
        
        mockHttp.When("http://localhost/locales/en/hello.json")
            .Respond("application/json", GetFile("locales/en/hello.json"));
        
        mockHttp.When("http://localhost/locales/de/hello.json")
            .Respond("application/json", GetFile("locales/de/hello.json"));
        
        mockHttp.When("http://localhost/locales/en/world.json")
            .Respond("application/json", GetFile("locales/en/world.json"));
        
        mockHttp.When("http://localhost/locales/de/world.json")
            .Respond("application/json", GetFile("locales/de/world.json"));
            

        var mockClient = mockHttp.ToHttpClient();
        mockClient.BaseAddress = new Uri("http://localhost/locales/");
        mockClient.Timeout = TimeSpan.FromSeconds(1);
        return mockClient;
    }
}