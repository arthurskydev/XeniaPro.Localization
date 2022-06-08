namespace XeniaPro.Localization.Models;

public record Language(string Name, string ShortHand)
{
    public override string ToString()
    {
        return $"{Name}, {ShortHand}";
    }
}