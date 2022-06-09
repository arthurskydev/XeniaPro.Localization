using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using XeniaPro.Localization.Abstractions;
using XeniaPro.Localization.Examples.BlazorWASM;
using XeniaPro.Localization.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddWebLocalization(options =>
{
    options.ResourceUrl = $"{builder.HostEnvironment.BaseAddress}/locales/";
    options.Languages = new List<Language>
    {
        new Language("Deutsch", "de"),
        new Language("English", "en")
    };
});

await builder.Build().RunAsync();