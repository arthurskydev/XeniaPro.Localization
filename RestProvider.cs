using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Xenia.SimpleRestClient;
using Xenia.SimpleRestClient.Exceptions;

namespace XeniaPro.Localization
{

    public class RestProvider : BaseProvider<RestProvider>, IAsyncLocalizationProvider
    {
        public class RestProviderOptions
        {
            public string BaseUrl { get; set; } = Constants.DefaultLocaleURL;
        }

        private readonly SimpleClient m_Client;

        public RestProvider(RestProviderOptions options, ILogger<RestProvider> logger) : base(logger)
        {
            m_Client = SimpleClient.CreateDefaultClient(options.BaseUrl);
        }

        public async Task Prefetch(string[] tables)
        {
            foreach (var table in tables)
            {
                await FetchTable(table);
            }
        }

        public async Task FetchTable(string tableKey)
        {
            SimpleResult<Dictionary<string, string>> result;
            try
            {
                result = await m_Client.GetAsync<Dictionary<string, string>>($"{tableKey}.json");
            }
            catch (DeserializerionException)
            {
                m_Logger.LogCritical("Locale table for {tableKey} seems to be incorrect or corrupted.", tableKey);
                return;
            }
            if (result.IsSuccessStatusCode && result.Body is not null)
            {
                m_Data[tableKey] = result.Body;
                m_Logger.LogInformation("Locale table for {tableKey} had been successfully loaded.", tableKey);
                return;
            }

            m_Logger.LogWarning("Could not find locale table for {tableKey}.", tableKey);
        }
    }
}
