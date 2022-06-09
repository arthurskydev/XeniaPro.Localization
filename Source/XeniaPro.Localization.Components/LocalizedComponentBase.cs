using Microsoft.AspNetCore.Components;
using XeniaPro.Localization.Abstractions;

namespace XeniaPro.Localization.Components;

public abstract class LocalizedComponentBase : ComponentBase
{
    [Inject] private IAsyncLocalizationProvider Provider { get; set; } = null!;
    [Inject] protected ILocalizer Localizer { get; set; } = null!;

    protected override void OnInitialized()
    {
        Provider.LocalesUpdated += StateHasChanged;
    }
}