using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using XeniaPro.Localization.Core.Interfaces;
using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.Core.Localizers;
using XeniaPro.Localization.Core.Options;

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
        foreach (var configureAction in config.ConfigureLocalizationProviders)
        {
            configureAction(services);
        };
        services.AddScoped<ILanguageProvider, LanguageProvider>();
        services.AddScoped<ILocalizer, Localizer>();
        return services;
    }
}

public class LocalizationConfiguration : LocalizationOptions
{
    public IEnumerable<Action<IServiceCollection>> ConfigureLocalizationProviders { get; set; }
}