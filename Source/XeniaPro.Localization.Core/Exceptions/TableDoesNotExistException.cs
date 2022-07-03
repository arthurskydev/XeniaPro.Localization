using System;
using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.Core.Models;

namespace XeniaPro.Localization.Core.Exceptions;

public class TableDoesNotExistException : Exception
{
    public TableDoesNotExistException(Language language) : base($"Table for {language} does not exist.") {}
    public TableDoesNotExistException(Language language, string @namespace) : base($"Table for {language}:{@namespace} does not exist.") {}
}