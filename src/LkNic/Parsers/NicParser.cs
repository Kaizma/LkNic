namespace LkNic;

internal static class NicParser
{
    private const int OldNicLength = 10;
    private const int NewNicLength = 12;

    public static Nic Parse(string nic)
    {
        Gender gender;
        int dayNumber;

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
            int rawDayNumber = int.Parse(nic.Substring(4, 3));
            (gender, dayNumber) = ParseDayNumber(rawDayNumber);
            return new Nic
            {
                BirthYear = int.Parse(nic[..4]),
                Gender = gender
            };
        }
        
        if (nic.Length == OldNicLength && (nic[..9].All(char.IsDigit)) && (nic.EndsWith("V", StringComparison.OrdinalIgnoreCase) || nic.EndsWith("X", StringComparison.OrdinalIgnoreCase)))
        {
            var year = int.Parse(nic[..2]);
            int rawDayNumber = int.Parse(nic.Substring(2, 3));
            (gender, dayNumber) = ParseDayNumber(rawDayNumber);
            return new Nic
            {
                BirthYear =  year + 1900,
                Gender = gender
            };
        }
        
        throw new ArgumentException("Invalid NIC"); 
    }

    private static (Gender Gender, int DayNumber) ParseDayNumber(int rawDayNumber)
    {
        Gender gender;
        int dayNumber;

        if (rawDayNumber > 500)
        {
            gender = Gender.Female;
            dayNumber = rawDayNumber - 500;
        }
        else
        {
            gender = Gender.Male;
            dayNumber = rawDayNumber;
        }

        if(dayNumber < 1 || dayNumber > 366)
        {
            throw new ArgumentException("Invalid day number in NIC.");
        }

        return (gender, dayNumber);
    }
}
