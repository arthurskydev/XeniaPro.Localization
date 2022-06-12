using System;
using System.Collections.Generic;
using XeniaPro.Localization.Core.LanguageProviders;

namespace XeniaPro.Localization.Core.Interfaces;

public interface ILanguageProvider
{
     event Action LanguageUpdated;
     
     Language CurrentLanguage { get; }
     
     ICollection<Language> Languages { get; }

     void SetLanguage(Language language);
}