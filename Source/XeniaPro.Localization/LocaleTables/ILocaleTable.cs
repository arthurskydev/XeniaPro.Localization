using XeniaPro.Localization.Models;

namespace XeniaPro.Localization.LocaleTables;

public interface ILocaleTable
{
    Language Language { get; }
    string GetByKey(string key);
}