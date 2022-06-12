using Microsoft.AspNetCore.Components;
using XeniaPro.Localization.Core.Interfaces;

namespace XeniaPro.Localization.Web;

public abstract class LocalizedComponentBase : ComponentBase
{
    [Inject] public IAsyncLocalizationProvider Provider { get; set; } = null!;
    [Inject] public ILanguageProvider LanguageProvider { get; set; } = null!;
    [Inject] public ILocalizer Localizer { get; set; } = null!;

    protected override void OnInitialized()
    {
        Provider.LocalesUpdated += StateHasChanged;
        LanguageProvider.LanguageUpdated += StateHasChanged;
    }
}