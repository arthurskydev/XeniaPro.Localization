using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using XeniaPro.Localization.Abstractions;
using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.Core.LocaleTables;
using XeniaPro.Localization.Core.Localizers;
using XeniaPro.Localization.UnitTests.Setup;

namespace XeniaPro.Localization.UnitTests.Tests;

public class MockLocalizationProvider : ILocalizationProvider
{
    public ILocaleTable GetTable(Language language)
    {
        var dict = TestSetup.GetDictionary(language.ShortHand);
        return new LocaleTable(dict, language);
    }
}

public class LocalizerTests
{
    private ILocalizer _localizer = null!;
    private ILanguageProvider _languageProvider = null!;

    [SetUp]
    public void Setup()
    {
        var options = Options.Create(TestSetup.Options);
        var logger = Mock.Of<ILogger<LanguageProvider>>();
        _languageProvider = new LanguageProvider(options, logger);
        var localeProvider = new MockLocalizationProvider();
        _localizer = new Localizer(localeProvider, _languageProvider, options);
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