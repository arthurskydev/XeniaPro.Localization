using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.Core.Models;

namespace XeniaPro.Localization.Core.Interfaces;

public interface ILocalizationProvider
{
    public ILocaleTable GetTable(Language language);
}