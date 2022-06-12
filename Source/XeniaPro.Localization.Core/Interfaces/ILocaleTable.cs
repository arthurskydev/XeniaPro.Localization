using XeniaPro.Localization.Core.LanguageProviders;

namespace XeniaPro.Localization.Core.Interfaces;

public interface ILocaleTable
{
    Language Language { get; }
    ILocaleItem GetItemByKey(string key);
}