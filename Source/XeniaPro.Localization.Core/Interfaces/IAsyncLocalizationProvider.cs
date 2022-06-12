using System;

namespace XeniaPro.Localization.Core.Interfaces;

public interface IAsyncLocalizationProvider : ILocalizationProvider
{
    public event Action LocalesUpdated;
}