using System.Text.RegularExpressions;

namespace LkNic;

internal static partial class NicParser
{
    [GeneratedRegex(@"^[0-9]{12}$")]
    private static partial Regex NewNicRegex();

    [GeneratedRegex(@"^[0-9]{9}[vVxX]$")]
    private static partial Regex OldNicRegex();

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

    public static bool TryParse(string? nicNumber, out Nic? nic)
    {
        return TryParseCore(nicNumber, out nic);
    }

    private static bool TryParseCore(string? nicNumber, out Nic? nic)
    {
        nic = null;

        if (string.IsNullOrWhiteSpace(nicNumber))
        {
            return false;
        }

        nicNumber = nicNumber.Trim().ToUpperInvariant();

        if (NewNicRegex().IsMatch(nicNumber))
        {
            var birthYear = int.Parse(nicNumber[..4]);
            var rawDayNumber = int.Parse(nicNumber[4..7]);

            if (!TryParseDayNumber(rawDayNumber, out var gender, out var dayNumber))
            {
                return false;
            }

            if (!TryGetBirthDate(birthYear, dayNumber, out var birthDate))
            {
                return false;
            }

            int ageInYears = CalculateAgeInYears(birthDate);

            nic = new Nic
            {
                Value = nicNumber,
                Type = NicType.New,
                BirthYear = birthYear,
                BirthDate = birthDate,
                Gender = gender,
                AgeInYears = ageInYears
            };

            return true;
        }

        else if (OldNicRegex().IsMatch(nicNumber))
        {
            var year = int.Parse(nicNumber[..2]);
            var rawDayNumber = int.Parse(nicNumber[2..5]);

            if (!TryParseDayNumber(rawDayNumber, out var gender, out var dayNumber))
            {
                return false;
            }

            var birthYear = 1900 + year;

            if (!TryGetBirthDate(birthYear, dayNumber, out var birthDate))
            {
                return false;
            }

            int ageInYears = CalculateAgeInYears(birthDate);

            nic = new Nic
            {
                Value = nicNumber,
                Type = NicType.Old,
                BirthYear = birthYear,
                BirthDate = birthDate,
                Gender = gender,
                AgeInYears = ageInYears
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

        birthDate = new DateOnly(year, 1, 1).AddDays(dayNumber - 1);
        return true;
    }

    private static int CalculateAgeInYears(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - birthDate.Year;
        if (birthDate.Month > today.Month || (birthDate.Month == today.Month && birthDate.Day > today.Day))
        {
            age--;
        }
        return age;
    }
}
