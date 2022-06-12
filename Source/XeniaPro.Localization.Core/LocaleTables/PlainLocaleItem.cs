using XeniaPro.Localization.Abstractions;

namespace XeniaPro.Localization.Core.LocaleTables;

public readonly struct PlainLocaleItem : ILocaleItem
{
    public string Key { get; }
    private string Value { get; }

    public PlainLocaleItem(string value, string key)
    {
        Value = value;
        Key = key;
    }

    public string GetString()
        => Value;

    public string GetString(string secondaryKey)
        => Value;
}