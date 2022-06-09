using Microsoft.Extensions.DependencyInjection;
using XeniaPro.Localization.Abstractions;
using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.Core.Localizers;

namespace XeniaPro.Localization.Files;

public static class DiExtensions
{
    public static IServiceCollection AddFileLocalization(this IServiceCollection services,
        Action<FileLocalizationOptions> options)
    {
        services.Configure(options);
        services.AddScoped<FileLocalizationProvider>();
        services.AddScoped<ILocalizationProvider>(p => p.GetRequiredService<FileLocalizationProvider>());
        services.AddScoped<IAsyncLocalizationProvider>(p => p.GetRequiredService<FileLocalizationProvider>());
        services.AddScoped<ILanguageProvider, LanguageProvider>();
        services.AddScoped<ILocalizer, Localizer>();
        return services;
    }

    public static IServiceCollection AddFileLocalization(this IServiceCollection services)
    {
        services.AddScoped<FileLocalizationProvider>();
        services.AddScoped<ILocalizationProvider>(p => p.GetRequiredService<FileLocalizationProvider>());
        services.AddScoped<IAsyncLocalizationProvider>(p => p.GetRequiredService<FileLocalizationProvider>());
        services.AddScoped<ILanguageProvider, LanguageProvider>();
        services.AddScoped<ILocalizer, Localizer>();
        return services;
    }
}

public class FileLocalizationOptions : ILocalizationOptions
{
    public string ResourcePath { get; set; }
    public string PlaceholderString { get; set; }
    public List<Language> Languages { get; set; }
}