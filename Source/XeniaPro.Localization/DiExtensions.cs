using Microsoft.Extensions.DependencyInjection;
using XeniaPro.Localization.LanguageProviders;
using XeniaPro.Localization.LocaleProviders;
using XeniaPro.Localization.Localizers;
using XeniaPro.Localization.Models;

namespace XeniaPro.Localization;

public static class DiExtensions
{
    public static IServiceCollection AddRestLocalization(this IServiceCollection services, RestLocalizationOptions options)
    {
        services.AddScoped<IAsyncLocalizationProvider, RestLocalizationProvider>(_ => new RestLocalizationProvider(options));
        services.AddScoped<ILocalizer, Localizer>();
        services.AddScoped<ILanguageProvider, LanguageProvider>(_ => new LanguageProvider(options));
        return services;
    }
    
}