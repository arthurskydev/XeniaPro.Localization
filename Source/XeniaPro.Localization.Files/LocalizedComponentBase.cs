using Microsoft.AspNetCore.Components;
using XeniaPro.Localization.Abstractions;

namespace XeniaPro.Localization.Files;

public abstract class LocalizedComponentBase : ComponentBase
{
    [Inject] public ILanguageProvider LanguageProvider { get; set; } = null!;
    [Inject] public ILocalizer Localizer { get; set; } = null!;

    protected override void OnInitialized()
    {
        LanguageProvider.LanguageUpdated += StateHasChanged;
    }
}