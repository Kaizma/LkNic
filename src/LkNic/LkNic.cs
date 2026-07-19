namespace LkNic;

public static class LkNic
{
    public static Nic Parse(string nic)
    {
        return NicParser.Parse(nic);
    }

    public static bool TryParse(string nicNumber, out Nic? nic)
    {
        return NicParser.TryParse(nicNumber, out nic);
    }
}
