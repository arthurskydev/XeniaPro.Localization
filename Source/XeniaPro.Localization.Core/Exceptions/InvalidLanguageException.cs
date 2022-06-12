using System;
using XeniaPro.Localization.Abstractions;

namespace XeniaPro.Localization.Core.Exceptions;

public class InvalidLanguageException : Exception
{
    public InvalidLanguageException(Language language) : base($"This language was not intended to be set: {language}.")
    {
        
    }
}