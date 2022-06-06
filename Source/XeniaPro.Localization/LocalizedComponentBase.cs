using Microsoft.AspNetCore.Components;

namespace XeniaPro.Localization;

public abstract class LocalizedComponentBase : ComponentBase
{
    [Inject] private IAsyncLocalizationProvider Provider { get; set; }
    [Inject] protected ILocalizer Localizer { get; set; }
    
    protected override void OnInitialized()
    {
        Provider.LanguagesUpdated += StateHasChanged;
    }
}