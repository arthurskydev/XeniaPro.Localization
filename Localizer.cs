using System;
using System.Collections.Generic;

namespace XeniaPro.Localization
{

    public class Localizer : ILocalizer
    {
        private readonly ILocalizationProvider m_Provider;
        private readonly ILanguageProvider m_Culture;

        public Localizer(ILanguageProvider culture, ILocalizationProvider provider)
        {
            m_Culture = culture;
            m_Provider = provider;
        }

        public string this[string idx] => GetString(idx);

        public Dictionary<string, string> GetAll(string locale)
        {
            throw new NotImplementedException();
        }

        public string GetString(string idx)
        {
            return m_Provider.GetString(m_Culture.GetCulture(), idx);
        }

        public void SetLocale(string locale)
        {
            Console.WriteLine("SetLocale for XeniaPro.Localizer: This feature is deprecieated and not in use anymore.");
        }
    }
}
