namespace XeniaPro.Localization.Abstractions;

public interface ILocaleTable
{
    Language Language { get; }
    string GetByKey(string key);
}