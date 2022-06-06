using System.Collections.Generic;
using System.Threading.Tasks;

namespace XeniaPro.Localization
{
    public interface ILocalizationProvider
    {
        Dictionary<string, string> GetTable(string tableKey);
        string GetString(string tableKey, string entry);
    }
}
