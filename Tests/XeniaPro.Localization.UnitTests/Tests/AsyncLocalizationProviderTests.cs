using XeniaPro.Localization.Abstractions;
using XeniaPro.Localization.Core.Exceptions;
using XeniaPro.Localization.UnitTests.Setup;

namespace XeniaPro.Localization.UnitTests.Tests;

public abstract class AsyncLocalizationProviderTests
{
    protected IAsyncLocalizationProvider Provider = null!;

    [Test]
    [TestCase("Deutsch", "de")]
    [TestCase("English", "en")]
    [Parallelizable(ParallelScope.None)]
    public async Task DoesUpdateAndProvideSet(string languageName, string languageShort)
    {
        var lang = new Language(languageName, languageShort);
        var completion = new TaskCompletionSource();
        Provider.LocalesUpdated += completion.SetResult;
        var table = Provider.GetTable(lang);
        Assert.That(table, Is.Not.Null);
        await completion.Task;
        var updatedTable = Provider.GetTable(lang);
        var random = new Random();
        var rndIdx = random.Next(TestSetup.GetDictionary(languageShort).Count);
        Assert.Multiple(() =>
        {
            Assert.That(updatedTable.Language, Is.EqualTo(lang));
            Assert.That(updatedTable.GetItemByKey(TestSetup.GetDictionary(languageShort).Keys.ElementAt(rndIdx)).GetString(),
                Is.EqualTo(TestSetup.GetDictionary(languageShort).Values.ElementAt(rndIdx)));
        });
    }

    [Test]
    public async Task ThrowsOnIncorrectLanguage()
    {
        var fooLang = new Language("foo", "bar");
        var completion = new TaskCompletionSource();
        Provider.LocalesUpdated += completion.SetResult;
        Provider.GetTable(fooLang);
        await completion.Task;
        Assert.Throws<TableDoesNotExistException>(() => Provider.GetTable(fooLang));
    }
}