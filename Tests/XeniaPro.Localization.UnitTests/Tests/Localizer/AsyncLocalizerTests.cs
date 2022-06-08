using Microsoft.Extensions.Options;
using XeniaPro.Localization.LanguageProviders;
using XeniaPro.Localization.LocaleProviders;
using XeniaPro.Localization.LocaleTables;
using XeniaPro.Localization.Localizers;
using XeniaPro.Localization.Models;
using XeniaPro.Localization.UnitTests.Setup;

namespace XeniaPro.Localization.UnitTests.Tests.Localizer;

public class MockLocaliationProvider : IAsyncLocalizationProvider
{
    public event Action? LanguagesUpdated;

    public ValueTask<ILocaleTable> GetTableAsync(Language language)
    {
        var dict = TestSetup.GetDictionary(language.ShortHand);
        var table = new LocaleTable(dict, language);
        return new ValueTask<ILocaleTable>(table);
    }
}

public class AsyncLocalizerTests
{
    private ILocalizer _localizer = null!;
    private ILanguageProvider _languageProvider = null!;

    [SetUp]
    public void Setup()
    {
        var options = Options.Create(TestSetup.RestOptions);
        _languageProvider = new LanguageProviders.LanguageProvider(options);
        var localeProvider = new MockLocaliationProvider();
        _localizer = new AsyncLocalizer(localeProvider, _languageProvider);
    }

    [Test]
    [TestCase("Deutsch", "de")]
    [TestCase("English", "en")]

    public void GetFromBracketsOverload(string langName, string langShort)
    {
        var lang = new Language(langName, langShort);
        _languageProvider.SetLanguage(lang);
        TestGet(key => _localizer[key], langShort);
    }

    private static void TestGet(Func<string, string> getFunc, string langCode)
    {
        var dict = TestSetup.GetDictionary(langCode);
        var random = new Random();
        var rndIdx = random.Next(dict.Count);
        var result = getFunc(dict.Keys.ElementAt(rndIdx));
        Assert.That(result, Is.EqualTo(dict.Values.ElementAt(rndIdx)));
    }
}