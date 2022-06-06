using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace XeniaPro.Localization;

public static class HostingExtensions
{
    public static IServiceCollection AddDefaultWebLocalization(this IServiceCollection services, RestProvider.RestProviderOptions options)
    {
        services.AddScoped<ILanguageProvider, DefaultLanguageProvider>();
        services.AddScoped<IAsyncLocalizationProvider, RestProvider>(services => new RestProvider(options, services.GetRequiredService<ILogger<RestProvider>>()));
        services.AddScoped<ILocalizer, Localizer>(services => new Localizer(services.GetRequiredService<ILanguageProvider>(), services.GetRequiredService<IAsyncLocalizationProvider>() as ILocalizationProvider));

        return services;
    }

    public static IServiceCollection AddDefaultFileLocalization(this IServiceCollection services, FileProvider.FileProviderOptions options)
    {
        services.AddScoped<ILanguageProvider, DefaultLanguageProvider>();
        services.AddScoped<ILocalizationProvider, FileProvider>(services => new FileProvider(options, services.GetRequiredService<ILogger<FileProvider>>()));
        services.AddScoped<ILocalizer, Localizer>();

        return services;
    }
}