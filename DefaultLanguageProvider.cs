using System.Globalization;

namespace XeniaPro.Localization;

public class DefaultLanguageProvider : ILanguageProvider
{
    public string GetCulture()
    {
        return CultureInfo.CurrentCulture.Name[0..1];
    }
}