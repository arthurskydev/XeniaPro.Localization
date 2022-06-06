using System.Threading.Tasks;

namespace XeniaPro.Localization;

public interface IAsyncLocalizationProvider : ILocalizationProvider
{
    Task Prefetch(string[] tables);
    Task FetchTable(string tableKey);
}