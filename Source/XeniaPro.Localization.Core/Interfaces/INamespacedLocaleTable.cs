using System.Collections.ObjectModel;

namespace XeniaPro.Localization.Core.Interfaces;

public interface INamespacedLocaleTable : ILocaleTable
{
    string Namespace { get; }
}
