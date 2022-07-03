using System.Globalization;

namespace XeniaPro.Localization.Core.Models;

public record Language(string Name, string LocaleName, CultureInfo CultureInfo)
{
    public override string ToString()
    {
        return $"{Name}, {LocaleName}, {CultureInfo.DisplayName}";
    }

    public static Language FromCultureInfo(CultureInfo cultureInfo, string localeName)
        => new(cultureInfo.DisplayName, localeName, cultureInfo);
}