using System;
using XeniaPro.Localization.Models;

namespace XeniaPro.Localization.Exceptions;

public class TableDoesNotExistException : Exception
{
    public TableDoesNotExistException(Language language) : base($"Table for {language} does not exist.") {}
}