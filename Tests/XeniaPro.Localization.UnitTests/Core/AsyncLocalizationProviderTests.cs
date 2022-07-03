using System.Globalization;
using XeniaPro.Localization.Core.Exceptions;
using XeniaPro.Localization.Core.Interfaces;
using XeniaPro.Localization.Core.Models;
using XeniaPro.Localization.UnitTests.Setup;

namespace XeniaPro.Localization.UnitTests.Core;

public abstract class AsyncLocalizationProviderTests
{
    protected IAsyncLocalizationProvider Provider = null!;

    [Test]
    [TestCase("Deutsch", "de")]
    [TestCase("English", "en")]
    [Parallelizable(ParallelScope.None)]
    public async Task DoesUpdateAndProvideSet(string languageName, string languageShort)
    {
        var lang = new Language(languageName, languageShort, CultureInfo.InvariantCulture);
        var completion = new TaskCompletionSource();
        Provider.LocalesUpdated += completion.SetResult;
        var table = Provider.GetTable(lang);
        Assert.That(table, Is.Not.Null);
        Assert.That(table.Language, Is.EqualTo(lang));
        await completion.Task;
        var updatedTable = Provider.GetTable(lang);
        Assert.That(updatedTable, Is.Not.Null);
        Assert.That(updatedTable.Language, Is.EqualTo(lang));
    }

    [Test]
    public async Task ThrowsOnIncorrectLanguage()
    {
        var fooLang = new Language("foo", "bar", CultureInfo.InvariantCulture);
        var completion = new TaskCompletionSource();
        Provider.LocalesUpdated += completion.SetResult;
        Provider.GetTable(fooLang);
        await completion.Task;
        Assert.That(() => Provider.GetTable(fooLang), Throws.Exception.TypeOf<TableDoesNotExistException>().Or.TypeOf<IndexNotFoundException>());
    }
}