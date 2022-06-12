using XeniaPro.Localization.Abstractions;
using XeniaPro.Localization.Core.Exceptions;
using XeniaPro.Localization.UnitTests.Setup;

namespace XeniaPro.Localization.UnitTests.Core;

public abstract class LocalizationProviderTests
{
    protected ILocalizationProvider Provider = null!;

    [Test]
    [TestCase("Deutsch", "de")]
    [TestCase("English", "en")]
    [Parallelizable(ParallelScope.None)]
    public void DoesUpdateAndProvideSet(string languageName, string languageShort)
    {
        var lang = new Language(languageName, languageShort);
        var table = Provider.GetTable(lang);
        var random = new Random();
        var rndIdx = random.Next(TestSetup.GetDictionary(languageShort).Count);
        Assert.Multiple(() =>
        {
            Assert.That(table.Language, Is.EqualTo(lang));
            Assert.That(table.GetItemByKey(TestSetup.GetDictionary(languageShort).Keys.ElementAt(rndIdx)).GetString(),
                Is.EqualTo(TestSetup.GetDictionary(languageShort).Values.ElementAt(rndIdx)));
        });
    }

    [Test]
    public void ThrowsOnIncorrectLanguage()
    {
        var fooLang = new Language("foo", "bar");
        Assert.Throws<TableDoesNotExistException>(() => Provider.GetTable(fooLang));
    }
}