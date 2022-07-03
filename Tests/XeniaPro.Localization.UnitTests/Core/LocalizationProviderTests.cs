using System.Globalization;
using XeniaPro.Localization.Core.Exceptions;
using XeniaPro.Localization.Core.Interfaces;
using XeniaPro.Localization.Core.Models;

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
        var lang = new Language(languageName, languageShort, CultureInfo.InvariantCulture);
        var table = Provider.GetTable(lang);
        Assert.That(table, Is.Not.Null);
        Assert.That(table.Language, Is.EqualTo(lang));
    }

    [Test]
    public void ThrowsOnIncorrectLanguage()
    {
        var fooLang = new Language("foo", "bar", CultureInfo.InvariantCulture);
        Assert.That(() => Provider.GetTable(fooLang), Throws.Exception.TypeOf<TableDoesNotExistException>().Or.TypeOf<IndexNotFoundException>());
    }
}