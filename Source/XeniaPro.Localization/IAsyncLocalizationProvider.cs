using System;

namespace XeniaPro.Localization;

public interface IAsyncLocalizationProvider
{
    public event Action LanguagesUpdated;
}