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
}