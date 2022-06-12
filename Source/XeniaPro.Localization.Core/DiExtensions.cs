using System;
using Microsoft.Extensions.DependencyInjection;
using XeniaPro.Localization.Abstractions;
using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.Core.Localizers;

namespace XeniaPro.Localization.Core;

public static class DiExtensions
{
    public static IServiceCollection AddXeniaLocalization(this IServiceCollection services,
        Action<LocalizationConfiguration> configure)
    {
        LocalizationConfiguration config = new();
        configure(config);
        services.Configure<LocalizationOptions>((options) =>
        {
            options.Languages = config.Languages;
            options.PlaceholderString = config.PlaceholderString;
        });
        config.ConfigureLocalizationProvider(services);
        services.AddScoped<ILanguageProvider, LanguageProvider>();
        services.AddScoped<ILocalizer, Localizer>();
        return services;
    }
}

public class LocalizationConfiguration : LocalizationOptions
{
    public Action<IServiceCollection> ConfigureLocalizationProvider { get; set; }
}