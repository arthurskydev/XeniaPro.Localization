using System;
using XeniaPro.Localization.Models;

namespace XeniaPro.Localization.Exceptions;

public class InvalidLanguageException : Exception
{
    public InvalidLanguageException(Language language) : base($"This language was not intended to be set: {language}.")
    {
        
    }
}