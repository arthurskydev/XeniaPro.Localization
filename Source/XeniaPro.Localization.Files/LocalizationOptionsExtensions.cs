using Microsoft.Extensions.DependencyInjection;
using XeniaPro.Localization.Core;
using XeniaPro.Localization.Core.Interfaces;

namespace XeniaPro.Localization.Files;

public static class LocalizationOptionsExtensions
{
    public static LocalizationConfiguration UseFileLocalization(this LocalizationConfiguration options,
        Action<FileLocalizationOptions> configureOptions)
    {
        options.ConfigureLocalizationProvider = (services) =>
        {
            services.Configure(configureOptions);
            services.AddScoped<FileLocalizationProvider>();
            services.AddScoped<ILocalizationProvider>(p => p.GetRequiredService<FileLocalizationProvider>());
        };

        return options;
    }

    public static LocalizationConfiguration UseFileLocalization(this LocalizationConfiguration options)
    {
        options.ConfigureLocalizationProvider = (services) =>
        {
            services.AddScoped<FileLocalizationProvider>();
            services.AddScoped<ILocalizationProvider>(p => p.GetRequiredService<FileLocalizationProvider>());
        };

        return options;
    }
}

public class FileLocalizationOptions
{
    public string ResourcePath { get; set; }
}