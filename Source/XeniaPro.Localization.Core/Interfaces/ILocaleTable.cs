using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.Core.Models;

namespace XeniaPro.Localization.Core.Interfaces;

public interface ILocaleTable
{
    Language Language { get; }
    ILocaleItem GetItemByKey(string key);
}