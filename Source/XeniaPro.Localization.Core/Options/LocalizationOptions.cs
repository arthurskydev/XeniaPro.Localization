using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using XeniaPro.Localization.Abstractions;

namespace XeniaPro.Localization.Core;

public class LocalizationOptions
{
    public string PlaceholderString { get; set; }
    public List<Language> Languages { get; set; }
}