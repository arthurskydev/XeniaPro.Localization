using XeniaPro.Localization.LocaleTables;
using XeniaPro.Localization.Models;

namespace XeniaPro.Localization.LocaleProviders;

public interface ILocalizationProvider
{
    public ILocaleTable GetTableFor(Language language);
}