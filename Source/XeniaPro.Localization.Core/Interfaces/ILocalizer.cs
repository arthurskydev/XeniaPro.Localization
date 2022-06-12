namespace XeniaPro.Localization.Core.Interfaces;

public interface ILocalizer
{
    public string this[string key] => Get(key);

    public string this[string key, params string[] args] => Get(key, args);

    public string Get(string key);

    public string Get(string key, params string[] args);
}