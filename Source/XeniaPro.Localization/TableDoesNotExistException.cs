using System;

namespace XeniaPro.Localization;

public class TableDoesNotExistException : Exception
{
    public TableDoesNotExistException(Language language) : base($"Table for {language.Name} does not exist.") {}
}