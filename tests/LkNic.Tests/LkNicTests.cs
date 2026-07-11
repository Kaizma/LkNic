namespace LkNic.Tests;

public class LkNicTests
{
    [Theory]
    [InlineData("200012345678", 2000)]
    [InlineData("199912345678", 1999)]
    [InlineData("202512345678", 2025)]
    [InlineData("752345678V", 1975)]
    [InlineData("902345678v", 1990)]
    [InlineData("852345678X", 1985)]
    [InlineData("992345678x", 1999)]
    public void Parse_ShouldReturnCorrectBirthYear_ForValidNic(string nic, int expectedBirthYear)
    {
        var result = LkNic.Parse(nic);
        Assert.Equal(expectedBirthYear, result.BirthYear);
    }

    [Theory]
    [InlineData("200000112345", 2000, 1, 1)]
    [InlineData("200003212345", 2000, 2, 1)]
    [InlineData("200006012345", 2000, 2, 29)]
    [InlineData("199906012345", 1999, 3, 1)]
    [InlineData("200050112345", 2000, 1, 1)]
    [InlineData("200053212345", 2000, 2, 1)]
    [InlineData("990010123V", 1999, 1, 1)]
    [InlineData("990320123V", 1999, 2, 1)]
    [InlineData("005600123V", 1900, 3, 1)]
    public void Parse_ShouldReturnCorrectBirthDate_ForValidNic(string nic, int year, int month, int day)
    {
        var result = LkNic.Parse(nic);

        Assert.Equal(new DateOnly(year, month, day), result.BirthDate);
    }

    [Fact]
    public void Parse_ShouldThrowArgumentNullException_WhenNicIsNull()
    {
        string? nic = null;
        Assert.Throws<ArgumentNullException>(() => LkNic.Parse(nic!));
    }

    [Theory]
    [InlineData("")]
    [InlineData("123456")]
    [InlineData("ABCDEFGHIJKL")]
    [InlineData("123456789A")]
    [InlineData("123456789Z")]
    [InlineData("2000ABC45678")]
    [InlineData("1234567890123")]
    public void Parse_ShouldThrowArgumentException_ForInvalidNic(string nic)
    {
        Assert.Throws<ArgumentException>(() => LkNic.Parse(nic));
    }

    [Theory]
    [InlineData("200012345678", Gender.Male)]
    [InlineData("200055678901", Gender.Female)]
    [InlineData("752345678V", Gender.Male)]
    [InlineData("755678901V", Gender.Female)]
    public void Parse_ShouldReturnCorrectGender_ForValidNic(string nic, Gender expectedGender)
    {
        var result = LkNic.Parse(nic);
        Assert.Equal(expectedGender, result.Gender);
    }

    [Theory]
    [InlineData("900001234V")]
    [InlineData("903671234V")]
    [InlineData("905001234V")]
    [InlineData("908671234V")]
    [InlineData("993660123V")]
    [InlineData("998660123V")]
    [InlineData("199936612345")]
    [InlineData("199986612345")]
    public void Parse_ShouldThrowException_ForInvalidDayNumber(string nicNumber)
    {
        Assert.Throws<ArgumentException>(() => LkNic.Parse(nicNumber));
    }
}
