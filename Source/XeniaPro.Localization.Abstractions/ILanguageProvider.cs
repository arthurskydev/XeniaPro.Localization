namespace XeniaPro.Localization.Abstractions;

public interface ILanguageProvider
{
     event Action LanguageUpdated;
     
     Language CurrentLanguage { get; }
     
     ICollection<Language> Languages { get; }

     void SetLanguage(Language language);
}