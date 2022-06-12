using XeniaPro.Localization.Abstractions;

namespace XeniaPro.Localization.Core.LocaleTables;

public readonly struct InvalidLocaleItem : ILocaleItem
{
    public string Key { get; }
    
    public InvalidLocaleItem(string key)
    {
        Key = key;
    }
    
    public string GetString()
        => string.Empty;

    public string GetString(string secondaryKey)
        => string.Empty;
}