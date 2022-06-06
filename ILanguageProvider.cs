using System.Collections.Generic;

namespace XeniaPro.Localization;

public interface ILanguageProvider
{
     Language CurrentLanguage { get; }
     
     ICollection<Language> GetLanguages { get; }

     void SetLanguage(Language language);
}