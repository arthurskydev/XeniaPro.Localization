namespace XeniaPro.Localization.Abstractions;

public interface ILocalizer
{
    public string this[string key] => Get(key);

    public string Get(string key);
}