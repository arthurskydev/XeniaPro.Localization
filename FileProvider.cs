using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xenia.SimpleRestClient.Exceptions;

namespace XeniaPro.Localization
{
    public class FileProvider : ILocalizationProvider
    {
        public class FileProviderOptions
        {
            public string ResroucePath { get; set; }
        }

        private readonly ILogger<FileProvider> m_Logger;
        private readonly Dictionary<string, Dictionary<string, string>> m_Data = new();
        
        private readonly FileProviderOptions m_Options;

        public FileProvider(FileProviderOptions options, ILogger<FileProvider> logger)
        {
            m_Logger = logger;
            m_Options = options;
        }

        public Dictionary<string, string> GetTable(string tableKey)
        {
            if (!m_Data.Keys.Contains(tableKey))
            {
                var result = LoadLocaleFile(tableKey);
                if (result is not null)
                {
                    m_Data[tableKey] = result;
                    return m_Data[tableKey];
                }
                return null;
            }

            return m_Data[tableKey];
        }

        public string GetString(string tableKey, string entryKey)
        {
            var table = GetTable(tableKey);
            if (table is not null)
            {
                if (table.TryGetValue(entryKey, out var entry))
                {
                    return entry;
                }
                m_Logger.LogInformation("Entry {entryKey} in table {tableKey} does not exist.", entryKey, tableKey);
                return string.Empty;
            }

            m_Logger.LogWarning("Table for {tableKey} was accessed but does not exist.", tableKey);
            return string.Empty;
        }
 
        private Dictionary<string, string> LoadLocaleFile(string localeKey)
        {
            var filePath = Path.Combine(m_Options.ResroucePath, localeKey + ".json");
            if (!File.Exists(filePath))
            {
                m_Logger.LogInformation("File not found for {localeKey}.", localeKey);
                return null;
            }
            var fileContent = File.ReadAllText(filePath);
            try
            {
                var deserialized = JsonSerializer.Deserialize<Dictionary<string, string>>(fileContent);
                m_Logger.LogInformation("File for {localeKey} was loaded successfully.", localeKey);
                return deserialized;
            }
            catch (DeserializerionException)
            {
                m_Logger.LogWarning("Locale file for {localeKey} seems damaged or incorrect.", localeKey);
                return null;
            }
        }
    }
}
