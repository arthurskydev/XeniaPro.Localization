using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.Files;
using XeniaPro.Localization.UnitTests.Core;
using XeniaPro.Localization.UnitTests.Setup;

namespace XeniaPro.Localization.UnitTests.Files;

public class FileLocalizationProviderTests : LocalizationProviderTests
{
    [SetUp]
    public void Setup()
    {
        var options = Options.Create(new FileLocalizationOptions
        {
            ResourcePath = $"{Directory.GetCurrentDirectory()}/"
        });
        var language = new LanguageProvider(Options.Create(TestSetup.Options), new NullLogger<LanguageProvider>());
        Provider = new FileLocalizationProvider(options, new NullLogger<FileLocalizationProvider>(), language);
    }
}