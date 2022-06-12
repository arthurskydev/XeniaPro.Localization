namespace XeniaPro.Localization.Abstractions;

public interface ILocaleItem
{
    string Key { get; }
    string GetString();
    string GetString(string secondaryKey);
}