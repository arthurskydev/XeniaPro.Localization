namespace XeniaPro.Localization.Abstractions;

public interface IAsyncLocalizationProvider : ILocalizationProvider
{
    public event Action LocalesUpdated;
}