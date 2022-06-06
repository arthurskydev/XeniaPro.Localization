namespace XeniaPro.Localization;

public interface ILocaleTable
{
    Language Language { get; }
    string GetByKey(string key);
}