namespace LkNic;

internal static class NicParser
{
    private const int OldNicLength = 10;
    private const int NewNicLength = 12;

    public static Nic Parse(string nic)
    {
        Gender gender;
        int dayNumber;
        DateOnly birthDate;

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
            int birthYear = int.Parse(nic[..4]);
            int rawDayNumber = int.Parse(nic.Substring(4, 3));
            (gender, dayNumber) = ParseDayNumber(rawDayNumber);
            birthDate = GetBirthDate(birthYear, dayNumber);

            return new Nic
            {
                BirthYear = birthYear,
                BirthDate = birthDate,
                Gender = gender
            };
        }
        
        if (nic.Length == OldNicLength && (nic[..9].All(char.IsDigit)) && (nic.EndsWith("V", StringComparison.OrdinalIgnoreCase) || nic.EndsWith("X", StringComparison.OrdinalIgnoreCase)))
        {
            var year = int.Parse(nic[..2]);
            int birthYear = year + 1900;
            int rawDayNumber = int.Parse(nic.Substring(2, 3));
            (gender, dayNumber) = ParseDayNumber(rawDayNumber);
            birthDate = GetBirthDate(birthYear, dayNumber);

            return new Nic
            {
                BirthYear = birthYear,
                BirthDate = birthDate,
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

    private static DateOnly GetBirthDate(int year, int dayNumber)
    {
        var daysInYear = DateTime.IsLeapYear(year) ? 366 : 365;
        if (dayNumber > daysInYear)
        {
            throw new ArgumentException("Invalid day number in NIC.", nameof(dayNumber));
        }

        return new DateOnly(year, 1, 1).AddDays(dayNumber - 1);
    }
}
