using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using XeniaPro.Localization.Files;
using XeniaPro.Localization.UnitTests.Core;

namespace XeniaPro.Localization.UnitTests.Files;

public class FileLocalizationProviderTests : AsyncLocalizationProviderTests
{
    [SetUp]
    public void Setup()
    {
        var options = Options.Create<FileLocalizationOptions>(new FileLocalizationOptions
        {
            ResourcePath = "/"
        });
        var mockLogger = Mock.Of<ILogger<FileLocalizationProvider>>();
        Provider = new FileLocalizationProvider(options, mockLogger);
    }
}