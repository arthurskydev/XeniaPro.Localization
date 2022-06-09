using RichardSzalay.MockHttp;
using XeniaPro.Localization.Exceptions;
using XeniaPro.Localization.LocaleProviders;
using XeniaPro.Localization.Models;
using XeniaPro.Localization.UnitTests.Setup;

namespace XeniaPro.Localization.UnitTests.Tests;



public class WebLocalizationProviderTests
{
    private IAsyncLocalizationProvider _provider = null!;

    private class TestProvider : WebLocalizationProvider
    {
        public TestProvider(HttpClient client) : base(client) { }
    }
    
    [SetUp]
    public void ProviderCustomClient()
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When("http://localhost/de.json")
            .Respond("application/json", TestSetup.GetLocaleFile("de"));

        mockHttp.When("http://localhost/en.json")
            .Respond("application/json", TestSetup.GetLocaleFile("en"));

        var mockClient = mockHttp.ToHttpClient();
        mockClient.BaseAddress = new Uri("http://localhost");
        mockClient.Timeout = TimeSpan.FromSeconds(1);
        _provider = new TestProvider(mockClient);
    }

    [Test]
    [TestCase("Deutsch", "de")]
    [TestCase("English", "en")]
    [Parallelizable(ParallelScope.None)]
    public async Task DoesUpdateAndProvideSet(string languageName, string languageShort)
    {
        var lang = new Language(languageName, languageShort);
        var table = await _provider.GetTableAsync(lang);
        var random = new Random();
        var rndIdx = random.Next(TestSetup.GetDictionary(languageShort).Count);
        Assert.Multiple(() =>
        {
            Assert.That(table.Language, Is.EqualTo(lang));
            Assert.That(table.GetByKey(TestSetup.GetDictionary(languageShort).Keys.ElementAt(rndIdx)),
                Is.EqualTo(TestSetup.GetDictionary(languageShort).Values.ElementAt(rndIdx)));
        });
    }

    [Test]
    public void ThrowsOnIncorrectLanguage()
    {
        var fooLang = new Language("foo", "bar");
        Assert.ThrowsAsync<TableDoesNotExistException>(async () => await _provider.GetTableAsync(fooLang));
    }
}