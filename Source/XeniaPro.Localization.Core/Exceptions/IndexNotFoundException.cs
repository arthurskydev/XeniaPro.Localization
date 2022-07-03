using System;
using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.Core.Models;

namespace XeniaPro.Localization.Core.Exceptions;

public class IndexNotFoundException : Exception
{
    public IndexNotFoundException(Language language) : base($"\".index\" for {language} was not found!")
    {
        
    }
}