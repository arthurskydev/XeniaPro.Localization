namespace XeniaPro.Localization.Core.Interfaces;

public interface ILocalizer
{
    public string this[string key] => Get(key);

    public string this[string key, params string[] args] => Get(key, args);

    public string this[string key, string @namespace] => Get(key, @namespace);

    public string this[string key, string @namespace, params string[] args] => Get(key, @namespace, args);

    public string Get(string key);

    public string Get(string key, params string[] args);

    public string Get(string key, string @namespace);

    public string Get(string key, string @namespace, params string[] args);
}