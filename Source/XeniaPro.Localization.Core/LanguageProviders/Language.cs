using System;
using System.Linq;

namespace XeniaPro.Localization.Core.LanguageProviders;

public record Language(string Name, string ShortHand)
{
    public override string ToString()
    {
        return $"{Name}, {ShortHand}";
    }

    public static implicit operator Language(string language)
    {
        var arr = language.Split(',', 2, StringSplitOptions.TrimEntries);
        return new Language(arr.First(), arr.Last());
    }
}