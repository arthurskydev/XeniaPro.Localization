using XeniaPro.Localization.Exceptions;
using XeniaPro.Localization.LanguageProviders;
using XeniaPro.Localization.Models;
using XeniaPro.Localization.UnitTests.Setup;

namespace XeniaPro.Localization.UnitTests.Tests;

public class LanguageProviderTests
{
    private ILanguageProvider _provider = null!;

    [SetUp]
    public void Setup()
    {
        _provider = new LanguageProvider(TestSetup.RestOptions);
    }

    [Test]
    public void DoesHaveLanguages()
    {
        Assert.That(_provider.GetLanguages, Is.EquivalentTo(TestSetup.RestOptions.Languages));
    }

    [Test, Order(1)]
    public void DoesSetDefaultLanguage()
    {
        Assert.That(_provider.CurrentLanguage, Is.EqualTo(TestSetup.RestOptions.Languages.First()));
    }

    [Test, Order(2)]
    [TestCase("English", "en")]
    public void DoesSetLanguage(string name, string shortName)
    {
        var language = new Language(name, shortName);
        _provider.SetLanguage(language);
        Assert.That(_provider.CurrentLanguage, Is.EqualTo(language));
    }

    [Test]
    public void DoesThrowOnInvalidLanguage()
    {
        var fooLang = new Language("foo", "bar");
        void SetLang() => _provider.SetLanguage(fooLang);
        Assert.Throws<InvalidLanguageException>(SetLang);
    }
}