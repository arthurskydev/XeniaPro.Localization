using XeniaPro.Localization.Core.LocaleIItems;

namespace XeniaPro.Localization.UnitTests.Core;

public class LocaleItemFactoryTests
{
    [Test]
    [TestCase("Hello World", "Hello World")]
    [TestCase("}} With escaped {{}} brackets {{", "} With escaped {} brackets {")]
    [TestCase("{{{{ escape one", "{{{ escape one")]
    [TestCase("{Interpolated:string} is so cool", "string is so cool")]
    [TestCase("{Interpolated:nesting {Is:cool}@} you must admit", "nesting cool you must admit")]
    public void DoesGetFullValue(string input, string expected)
    {
        var localeItem = LocaleItemFactory.CreateFromKeyValue("foo", input);
        Assert.That(localeItem.GetString(), Is.EqualTo(expected));
    }

    [Test]
    [TestCase("Hello World", "Hello World", "foo", "bar")]
    [TestCase("Hello {Name:Arthur}!", "Arthur", "Name")]
    [TestCase("Hello {Name:Arthur}!", "Hello ", "0")]
    [TestCase("Hello {Name:Arthur}!", "!", "1")]
    [TestCase("Super {This:Is{Very:Nested {As:You} can see}@} nesting", "You", "This", "Very", "As")]
    [TestCase("Super {This:Is{Very:Nested {As:You} can see}@} nesting", "", "Foo", "Bar", "FooBar")]

    public void DoesGetWithArgs(string input, string expected, params string[] args)
    {
        var localeItem = LocaleItemFactory.CreateFromKeyValue("foo", input);
        Assert.That(localeItem.GetString(args), Is.EqualTo(expected));
    }
}