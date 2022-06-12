using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using XeniaPro.Localization.Abstractions;
using XeniaPro.Localization.Core;
using XeniaPro.Localization.Examples.BlazorWASM;
using XeniaPro.Localization.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddXeniaLocalization(options =>
{
    // If the locale string can not be found use this value instead. A "." would pass the key through.
    options.PlaceholderString = "";

    /*
     * Preferably load this from a configuration file.
     * 
     * The fist language will be picked as default.
     *
     * Make sure to manually cache the selected language on your client.
     */
    options.Languages = new List<Language>
    {
        "de-DE, de",
        "en-EN, en"
    };

    options.UseWebLocalization(webOptions =>
    {
        // Place your locale files in a directory "locales" in "wwwroot".
        webOptions.ResourceUrl = $"{builder.HostEnvironment.BaseAddress}/locales/";
    });
});

await builder.Build().RunAsync();