namespace LkNic;

internal static class NicParser
{
    private const int OldNicLength = 10;
    private const int NewNicLength = 12;

    public static Nic Parse(string nic)
    {
        if(nic == null)
        {
            ArgumentNullException.ThrowIfNull(nic);
        }

        if(string.IsNullOrWhiteSpace(nic))
        {
            throw new ArgumentException("NIC cannot be empty or whitespace.", nameof(nic));
        }

        if (nic.Length == NewNicLength && nic.All(char.IsDigit))
        {
            return new Nic
            {
                BirthYear = int.Parse(nic[..4])
            };
        }
        
        if (nic.Length == OldNicLength && (nic[..9].All(char.IsDigit)) && (nic.EndsWith("V", StringComparison.OrdinalIgnoreCase) || nic.EndsWith("X", StringComparison.OrdinalIgnoreCase)))
        {
            var year = int.Parse(nic[..2]);
            return new Nic
            {
                BirthYear =  year + 1900
            };
        }
        
        throw new ArgumentException("Invalid NIC"); 
    }
}
