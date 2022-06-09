using Microsoft.Extensions.DependencyInjection;
using XeniaPro.Localization.Abstractions;
using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.Core.Localizers;

namespace XeniaPro.Localization.Web;

public static class DiExtensions
{
    public static IServiceCollection AddWebLocalization(this IServiceCollection services,
        Action<WebLocalizationOptions> options)
    {
        services.Configure(options);
        services.AddScoped<WebLocalizationProvider>();
        services.AddScoped<ILocalizationProvider>(p => p.GetRequiredService<WebLocalizationProvider>());
        services.AddScoped<IAsyncLocalizationProvider>(p => p.GetRequiredService<WebLocalizationProvider>());
        services.AddScoped<ILanguageProvider, LanguageProvider>();
        services.AddScoped<ILocalizer, Localizer>();
        return services;
    }

    public static IServiceCollection AddWebLocalization(this IServiceCollection services)
    {
        services.AddScoped<WebLocalizationProvider>();
        services.AddScoped<ILocalizationProvider>(p => p.GetRequiredService<WebLocalizationProvider>());
        services.AddScoped<IAsyncLocalizationProvider>(p => p.GetRequiredService<WebLocalizationProvider>());
        services.AddScoped<ILanguageProvider, LanguageProvider>();
        services.AddScoped<ILocalizer, Localizer>();
        return services;
    }
}

public class WebLocalizationOptions : ILocalizationOptions
{
    public string ResourceUrl { get; set; }
    public string PlaceholderString { get; set; }
    public List<Language> Languages { get; set; }
}