namespace XeniaPro.Localization.Abstractions.Exceptions;

public class TableDoesNotExistException : Exception
{
    public TableDoesNotExistException(Language language) : base($"Table for {language} does not exist.") {}
}