using XeniaPro.Localization.LocaleTables;
using XeniaPro.Localization.Models;
using XeniaPro.Localization.UnitTests.Setup;

namespace XeniaPro.Localization.UnitTests.Tests;

public class LocaleTableTests
{
    private readonly Random _random = new();
    private ILocaleTable _table = null!;
    
    [SetUp]
    public void Setup()
    {
        var language = new Language("Deutsch", "de");
        _table = new LocaleTable(TestSetup.Strings, language);
    }

    [Test]
    public void DoesGetValueByKey()
    {
        var rndIdx = _random.Next(TestSetup.Strings.Count);
        Assert.That(_table.GetByKey(TestSetup.Strings.Keys.ElementAt(rndIdx)), 
            Is.EqualTo(TestSetup.Strings.Values.ElementAt(rndIdx)));
    }
    
    [Test]
    public void DoesGetEmptyIfNoCorrespondingEntry()
    {
        Assert.That(_table.GetByKey("foo"), Is.EqualTo(string.Empty));
    }
}