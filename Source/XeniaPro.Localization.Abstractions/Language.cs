namespace XeniaPro.Localization.Abstractions;

public record Language
{
    public string Name { get; set; }
    public string ShortHand { get; set; }


    public Language(string name, string shortHand)
    {
        Name = name;
        ShortHand = shortHand;
    }

    public Language()
    {
        
    }
    
    public override string ToString()
    {
        return $"{Name}, {ShortHand}";
    }
}