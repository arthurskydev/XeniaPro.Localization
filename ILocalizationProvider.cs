namespace XeniaPro.Localization;

public interface ILocalizationProvider
{
    public ILocaleTable GetTableFor(Language language);
}