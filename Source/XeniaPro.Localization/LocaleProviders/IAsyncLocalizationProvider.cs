using System;
using System.Threading.Tasks;
using XeniaPro.Localization.LocaleTables;
using XeniaPro.Localization.Models;

namespace XeniaPro.Localization.LocaleProviders;

public interface IAsyncLocalizationProvider
{
    public event Action LanguagesUpdated;
    
    public ValueTask<ILocaleTable> GetTableAsync(Language language);
}