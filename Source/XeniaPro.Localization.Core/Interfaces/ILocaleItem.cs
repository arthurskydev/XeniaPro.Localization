namespace XeniaPro.Localization.Core.Interfaces;

public interface ILocaleItem
{
    string Key { get; }
    string GetString();
    string GetString(params string[] args);
}