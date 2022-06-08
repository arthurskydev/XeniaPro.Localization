using System.Collections.Generic;
using XeniaPro.Localization.Models;

namespace XeniaPro.Localization.LanguageProviders;

public interface ILanguageProvider
{
     Language CurrentLanguage { get; }
     
     ICollection<Language> GetLanguages { get; }

     void SetLanguage(Language language);
}