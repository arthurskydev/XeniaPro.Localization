using System;
using XeniaPro.Localization.Abstractions;

namespace XeniaPro.Localization.Core.Exceptions;

public class TableDoesNotExistException : Exception
{
    public TableDoesNotExistException(Language language) : base($"Table for {language} does not exist.") {}
}