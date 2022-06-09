namespace XeniaPro.Localization.Abstractions;

public interface ILocalizationProvider
{
    public ILocaleTable GetTable(Language language);
}