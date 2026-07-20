namespace LkNic;

public class Nic
{
    public string Value { get; init; } = string.Empty;
    public NicType Type { get; init; }
    public int BirthYear { get; init; }
    public DateOnly BirthDate { get; init; }
    public Gender Gender { get; init; }
    public int AgeInYears { get; init; }
}
