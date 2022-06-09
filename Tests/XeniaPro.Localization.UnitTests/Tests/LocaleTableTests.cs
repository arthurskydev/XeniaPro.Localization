using XeniaPro.Localization.Abstractions;
using XeniaPro.Localization.Core.LocaleTables;
using XeniaPro.Localization.UnitTests.Setup;

namespace XeniaPro.Localization.UnitTests.Tests;

public class LocaleTableTests
{
    private readonly Random _random = new();
    private ILocaleTable _table = null!;
    private Language? _language;
    
    [SetUp]
    public void Setup()
    {
        _language = new Language("Deutsch", "de");
        _table = new LocaleTable(TestSetup.GetDictionary(_language.ShortHand), _language);
    }

    [Test]
    public void DoesGetValueByKey()
    {
        var dict = TestSetup.GetDictionary(_language!.ShortHand);
        var rndIdx = _random.Next(dict.Count);
        Assert.That(_table.GetByKey(dict.Keys.ElementAt(rndIdx)), 
            Is.EqualTo(dict.Values.ElementAt(rndIdx)));
    }
    
    [Test]
    public void DoesGetEmptyIfNoCorrespondingEntry()
    {
        Assert.That(_table.GetByKey("foo"), Is.EqualTo(string.Empty));
    }
}