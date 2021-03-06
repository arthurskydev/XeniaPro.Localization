using System.Globalization;
using XeniaPro.Localization.Core.Interfaces;
using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.Core.LocaleTables;
using XeniaPro.Localization.Core.Models;
using XeniaPro.Localization.UnitTests.Setup;

namespace XeniaPro.Localization.UnitTests.Core;

public class LocaleTableTests
{
    private readonly Random _random = new();
    private ILocaleTable _table = null!;
    private Language? _language;
    
    [SetUp]
    public void Setup()
    {
        _language = new Language("Deutsch", "de", CultureInfo.InvariantCulture);
        _table = new LocaleTable(TestSetup.GetDictionary(_language.LocaleName), _language);
    }

    [Test]
    public void DoesGetValueByKey()
    {
        var dict = TestSetup.GetDictionary(_language!.LocaleName);
        var rndIdx = _random.Next(dict.Count);
        Assert.That(_table.GetItemByKey(dict.Keys.ElementAt(rndIdx)).GetString(), 
            Is.EqualTo(dict.Values.ElementAt(rndIdx)));
    }
    
    [Test]
    public void DoesGetEmptyIfNoCorrespondingEntry()
    {
        Assert.That(_table.GetItemByKey("foo").GetString(), Is.EqualTo(string.Empty));
    }
}