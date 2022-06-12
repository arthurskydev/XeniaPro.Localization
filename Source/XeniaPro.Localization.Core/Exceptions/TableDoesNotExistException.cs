using System;
using XeniaPro.Localization.Core.LanguageProviders;

namespace XeniaPro.Localization.Core.Exceptions;

public class TableDoesNotExistException : Exception
{
    public TableDoesNotExistException(Language language) : base($"Table for {language} does not exist.") {}
}