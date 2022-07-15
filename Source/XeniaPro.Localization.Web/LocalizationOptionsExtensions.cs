using Microsoft.Extensions.DependencyInjection;
using XeniaPro.Localization.Core;
using XeniaPro.Localization.Core.Interfaces;

namespace XeniaPro.Localization.Web;

public static class LocalizationOptionsExtensions
{
    public static LocalizationConfiguration UseWebLocalization(this LocalizationConfiguration options,
        Action<WebLocalizationOptions> configureOptions)
    {
        options.ConfigureLocalizationProvider = (services) =>
        {
            services.Configure(configureOptions);
            services.AddScoped<ILocalizationProvider, WebLocalizationProvider>();
        };
        return options;
    }

    public static LocalizationConfiguration UseWebLocalization(this LocalizationConfiguration options)
    {
        options.ConfigureLocalizationProvider = (services) =>
        {
            services.AddScoped<ILocalizationProvider, WebLocalizationProvider>();
        };
        return options;
    }
}

public class WebLocalizationOptions
{
    public string ResourceUrl { get; set; }
}