using Microsoft.Extensions.DependencyInjection;

namespace XeniaPro.Localization;

public static class DiExtensions
{
    public static IServiceCollection AddRestLocalization(this IServiceCollection services, RestLocalizationOptions options)
    {
        services.AddScoped<ILocalizationProvider, RestLocalizationProvider>(_ => new RestLocalizationProvider(options));
        services.AddScoped<ILocalizer, Localizer>();
        services.AddScoped<ILanguageProvider, LanguageProvider>(_ => new LanguageProvider(options));
        return services;
    }
}