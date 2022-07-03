using System;
using System.Collections.ObjectModel;

namespace XeniaPro.Localization.Core.Interfaces;

public interface INamespacedLocaleTable : ILocaleTable
{
    ReadOnlyCollection<string> Namespaces { get; }
    
    ILocaleItem GetNamespacedItemByKey(string @namespace, string key);
}
