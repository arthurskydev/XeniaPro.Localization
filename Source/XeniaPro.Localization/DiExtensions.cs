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
        Action<RestLocalizationOptions> options)
    {
        services.Configure(options);
        services.AddScoped<IAsyncLocalizationProvider, RestLocalizationProvider>();
        services.AddScoped<ILocalizer, AsyncLocalizer>();
        services.AddScoped<ILanguageProvider, LanguageProvider>();
        return services;
    }

    public static IServiceCollection AddRestLocalization(this IServiceCollection services)
    {
        services.AddScoped<IAsyncLocalizationProvider, RestLocalizationProvider>();
        services.AddScoped<ILocalizer, AsyncLocalizer>();
        services.AddScoped<ILanguageProvider, LanguageProvider>();
        return services;
    }
}

public class RestLocalizationOptions
{
    public string ResourceUrl { get; set; }
    public List<Language> Languages { get; set; }
}