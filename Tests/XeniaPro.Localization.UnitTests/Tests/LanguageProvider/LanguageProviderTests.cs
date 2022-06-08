using Microsoft.Extensions.Options;
using XeniaPro.Localization.Exceptions;
using XeniaPro.Localization.LanguageProviders;
using XeniaPro.Localization.Models;
using XeniaPro.Localization.UnitTests.Setup;

namespace XeniaPro.Localization.UnitTests.Tests.LanguageProvider;

[TestFixture]
public class LanguageProviderTests
{
    private ILanguageProvider _provider = null!;
    
    [SetUp]
    public void MsOptions()
    {
        var options = Options.Create(TestSetup.WebOptions);
        _provider = new LanguageProviders.LanguageProvider(options);
    }

    [Test]
    public void DoesHaveLanguages()
    {
        Assert.That(_provider.Languages, Is.EquivalentTo(TestSetup.WebOptions.Languages));
    }

    [Test, Order(1)]
    public void DoesSetDefaultLanguage()
    {
        Assert.That(_provider.CurrentLanguage, Is.EqualTo(TestSetup.WebOptions.Languages.First()));
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