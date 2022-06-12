using XeniaPro.Localization.Abstractions;

namespace XeniaPro.Localization.Core.LocaleIItems;

public readonly struct PlainLocaleItem : ILocaleItem
{
    public string Key { get; }
    private string Value { get; }

    public PlainLocaleItem(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string GetString()
        => Value;

    public string GetString(params string[] args)
        => Value;
}