namespace LkNic;

internal static class NicParser
{
    private const int OldNicLength = 10;
    private const int NewNicLength = 12;

    public static Nic Parse(string nicNumber)
    {
        if (nicNumber is null)
        {
            throw new ArgumentNullException(nameof(nicNumber));
        }

        if (!TryParseCore(nicNumber, out var nic))
        {
            if (string.IsNullOrWhiteSpace(nicNumber))
            {
                throw new ArgumentException("NIC cannot be empty or whitespace.", nameof(nicNumber));
            }

            throw new ArgumentException("Invalid NIC", nameof(nicNumber));
        }

        return nic!;
    }

    public static bool TryParse(string nicNumber, out Nic? nic)
    {
        return TryParseCore(nicNumber, out nic);
    }

    private static bool TryParseCore(string nicNumber, out Nic? nic)
    {
        nic = null;

        if (string.IsNullOrWhiteSpace(nicNumber))
        {
            return false;
        }

        if (nicNumber.Length == NewNicLength && nicNumber.All(char.IsDigit))
        {
            if (!int.TryParse(nicNumber[..4], out var birthYear)
                || !int.TryParse(nicNumber.Substring(4, 3), out var rawDayNumber)
                || !TryParseDayNumber(rawDayNumber, out var gender, out var dayNumber)
                || !TryGetBirthDate(birthYear, dayNumber, out var birthDate))
            {
                return false;
            }

            nic = new Nic
            {
                BirthYear = birthYear,
                BirthDate = birthDate,
                Gender = gender
            };

            return true;
        }
        
        if (nicNumber.Length == OldNicLength
            && nicNumber[..9].All(char.IsDigit)
            && (nicNumber.EndsWith("V", StringComparison.OrdinalIgnoreCase)
                || nicNumber.EndsWith("X", StringComparison.OrdinalIgnoreCase)))
        {
            if (!int.TryParse(nicNumber[..2], out var year)
                || !int.TryParse(nicNumber.Substring(2, 3), out var rawDayNumber))
            {
                return false;
            }

            var birthYear = year + 1900;
            if (!TryParseDayNumber(rawDayNumber, out var gender, out var dayNumber)
                || !TryGetBirthDate(birthYear, dayNumber, out var birthDate))
            {
                return false;
            }

            nic = new Nic
            {
                BirthYear = birthYear,
                BirthDate = birthDate,
                Gender = gender
            };

            return true;
        }
        
        return false;
    }

    private static bool TryParseDayNumber(int rawDayNumber, out Gender gender, out int dayNumber)
    {
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

        if (dayNumber < 1 || dayNumber > 366)
        {
            return false;
        }

        return true;
    }

    private static bool TryGetBirthDate(int year, int dayNumber, out DateOnly birthDate)
    {
        birthDate = default;

        var daysInYear = DateTime.IsLeapYear(year) ? 366 : 365;
        if (dayNumber > daysInYear)
        {
            return false;
        }

        try
        {
            birthDate = new DateOnly(year, 1, 1).AddDays(dayNumber - 1);
            return true;
        }
        catch (ArgumentOutOfRangeException)
        {
            return false;
        }
    }
}
