namespace XeniaPro.Localization.Abstractions;

public interface ILocaleTable
{
    Language Language { get; }
    ILocaleItem GetItemByKey(string key);
}