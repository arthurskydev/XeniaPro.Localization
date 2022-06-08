using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using XeniaPro.Localization.LanguageProviders;
using XeniaPro.Localization.LocaleProviders;
using XeniaPro.Localization.Localizers;
using XeniaPro.Localization.Models;

namespace XeniaPro.Localization;

public static class DiExtensions
{
    public static IServiceCollection AddRestLocalization(this IServiceCollection services,
        Action<WebLocalizationOptions> options)
    {
        services.Configure(options);
        services.AddScoped<IAsyncLocalizationProvider, WebLocalizationProvider>();
        services.AddScoped<ILocalizer, AsyncLocalizer>();
        services.AddScoped<ILanguageProvider, LanguageProvider>();
        return services;
    }

    public static IServiceCollection AddRestLocalization(this IServiceCollection services)
    {
        services.AddScoped<IAsyncLocalizationProvider, WebLocalizationProvider>();
        services.AddScoped<ILocalizer, AsyncLocalizer>();
        services.AddScoped<ILanguageProvider, LanguageProvider>();
        return services;
    }
}

public class WebLocalizationOptions
{
    public string ResourceUrl { get; set; }
    public List<Language> Languages { get; set; }
}