using System.Collections.Generic;

namespace XeniaPro.Localization.Models;

public class RestLocalizationOptions
{
    public string HostUri { get; set; }
    public List<Language> Languages { get; set; }
}