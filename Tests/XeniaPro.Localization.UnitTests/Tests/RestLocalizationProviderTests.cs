using System.Text.Json;
using RichardSzalay.MockHttp;
using XeniaPro.Localization.LocaleProviders;
using XeniaPro.Localization.Models;

namespace XeniaPro.Localization.UnitTests.Tests;

public class RestLocalizationProviderTests
{
    private IAsyncLocalizationProvider _provider = null!;
    private Dictionary<string, string> _strings = null!;

    [SetUp]
    public void Setup()
    {
        var deJson = File.ReadAllText("de.json");
        _strings = JsonSerializer.Deserialize<Dictionary<string, string>>(deJson)!;
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When("http://localhost/de.json")
                .Respond("application/json", deJson);
        
        var mockClient = mockHttp.ToHttpClient();
        mockClient.BaseAddress = new Uri("http://localhost");
        mockClient.Timeout = TimeSpan.FromSeconds(1);
        _provider = new RestLocalizationProvider(mockClient);
    }

    [Test]
    [TestCase("Deutsch", "de")]
    [Parallelizable(ParallelScope.None)]
    public async Task DoesUpdateLanguagesOnSuccessfulFetch(string languageName, string languageShort)
    {
        var lang = new Language(languageName, languageShort);
        var completion = new TaskCompletionSource();
        _provider.LanguagesUpdated += completion.SetResult;
        var emptyTable = _provider.GetTableFor(lang);
        Assert.That(emptyTable.Language, Is.EqualTo(lang));
        await completion.Task;
        var table = _provider.GetTableFor(lang);

        var random = new Random();
        var rndIdx = random.Next(_strings.Count);
        Assert.Multiple(() =>
        {
            Assert.That(table.Language, Is.EqualTo(lang));
            Assert.That(table.GetByKey(_strings.Keys.ElementAt(rndIdx)), 
                Is.EqualTo(_strings.Values.ElementAt(rndIdx)));
        });
    }
}