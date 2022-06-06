using System.Collections.Generic;

namespace XeniaPro.Localization;

public record RestLocalizationOptions(string HostUri, List<Language> Languages);