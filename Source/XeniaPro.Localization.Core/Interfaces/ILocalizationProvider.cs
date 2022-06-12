using XeniaPro.Localization.Core.LanguageProviders;

namespace XeniaPro.Localization.Core.Interfaces;

public interface ILocalizationProvider
{
    public ILocaleTable GetTable(Language language);
}