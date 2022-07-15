using System;
using System.Threading.Tasks;
using XeniaPro.Localization.Core.Models;

namespace XeniaPro.Localization.Core.Interfaces;

public interface ILocalizationProvider
{
    public event Action LocalesUpdated;

    public Task<ILocaleTable> GetTableAsync(Language language);

    public Task<INamespacedLocaleTable> GetTableAsync(Language language, string @namepace);
}