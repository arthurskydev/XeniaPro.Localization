using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace XeniaPro.Localization;

public abstract class BaseProvider<TProvider> : ILocalizationProvider
{

    protected readonly ILogger<TProvider> m_Logger;
    protected readonly Dictionary<string, Dictionary<string, string>> m_Data = new();

    public BaseProvider(ILogger<TProvider> logger)
    {
        m_Logger = logger;
    }

    public Dictionary<string, string> GetTable(string tableKey)
    {
        if (m_Data.TryGetValue(tableKey, out var table))
        {
            return table;
        }

        return null;
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
            return string.Empty;
        }

        return string.Empty;
    }
}