namespace LkNic;

public static class LkNic
{
    public static Nic Parse(string nic)
    {
        return NicParser.Parse(nic);
    }
}