using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using XeniaPro.Localization.UnitTests.Core;
using XeniaPro.Localization.UnitTests.Setup;
using XeniaPro.Localization.Web;

namespace XeniaPro.Localization.UnitTests.Web;

public class WebLocalizationProviderTests : AsyncLocalizationProviderTests
{
    private class TestProvider : WebLocalizationProvider
    {
        public TestProvider(HttpClient client, ILogger<WebLocalizationProvider> logger) : base(client, logger) { }
    }
    
    
    [SetUp]
    public void Setup()
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When("http://localhost/de.json")
            .Respond("application/json", TestSetup.GetLocaleFile("de"));

        mockHttp.When("http://localhost/en.json")
            .Respond("application/json", TestSetup.GetLocaleFile("en"));

        var mockClient = mockHttp.ToHttpClient();
        var mockLogger = Mock.Of<ILogger<WebLocalizationProvider>>();
        mockClient.BaseAddress = new Uri("http://localhost");
        mockClient.Timeout = TimeSpan.FromSeconds(1);
        Provider = new TestProvider(mockClient, mockLogger);
    }
}