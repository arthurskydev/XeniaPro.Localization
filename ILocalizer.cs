using System.Collections.Generic;

namespace XeniaPro.Localization
{
    public interface ILocalizer
    {
        string this[string idx] => GetString(idx);
        string GetString(string idx);
        void SetLocale(string locale);
        Dictionary<string, string> GetAll(string locale);
    }
}
