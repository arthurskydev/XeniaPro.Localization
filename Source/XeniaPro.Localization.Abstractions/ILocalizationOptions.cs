namespace XeniaPro.Localization.Abstractions;

public interface ILocalizationOptions
{
    public string PlaceholderString { get; }
    public List<Language> Languages { get; }
}