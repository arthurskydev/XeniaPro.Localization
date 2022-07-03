using System.Globalization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using XeniaPro.Localization.Core.LanguageProviders;
using XeniaPro.Localization.Core.Options;
using XeniaPro.Localization.UnitTests.Core;
using XeniaPro.Localization.UnitTests.Setup;
using XeniaPro.Localization.Web;

namespace XeniaPro.Localization.UnitTests.Web;

public class WebLocalizationProviderTests : AsyncLocalizationProviderTests
{
    private class TestProvider : WebLocalizationProvider
    {
        public TestProvider(HttpClient client) : base(client, NullLogger<WebLocalizationProvider>.Instance, Options.Create(TestSetup.Options)) { }
    }
    
    
    [SetUp]
    public void Setup()
    {
        Provider = new TestProvider(TestSetup.GetClient());
    }
}

public class WebLocalizationProviderTestsWOptions : AsyncLocalizationProviderTests
{
    private class TestProvider : WebLocalizationProvider
    {
        public TestProvider(HttpClient client, LocalizationOptions options) : base(client, NullLogger<WebLocalizationProvider>.Instance, Options.Create(options)) { }
    }
    
    
    [SetUp]
    public void Setup()
    {
        var options = TestSetup.Options;
        options.UseNamespaces();
        Provider = new TestProvider(TestSetup.GetClient(), options);
    }
}