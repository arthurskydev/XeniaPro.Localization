using System;

namespace XeniaPro.Localization.LocaleProviders;

public interface IAsyncLocalizationProvider : ILocalizationProvider
{
    public event Action LanguagesUpdated;
}