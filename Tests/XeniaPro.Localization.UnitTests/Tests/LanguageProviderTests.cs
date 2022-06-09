using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using XeniaPro.Localization.Abstractions;
using XeniaPro.Localization.Abstractions.Exceptions;
using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.UnitTests.Setup;

namespace XeniaPro.Localization.UnitTests.Tests;

[TestFixture]
public class LanguageProviderTests
{
    private ILanguageProvider _provider = null!;
    
    [SetUp]
    public void MsOptions()
    {
        var logger = Mock.Of<ILogger<LanguageProvider>>();
        var options = Options.Create(TestSetup.WebOptions);
        _provider = new LanguageProvider(options, logger);
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