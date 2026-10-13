using System.ComponentModel.DataAnnotations;
using Pianista.Core.Songs;

namespace Pianista.Tests.Songs;

public sealed class DurationTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void RejectsLengthThatIsNotPositive(int milliseconds)
    {
        Assert.Throws<ValidationException>(() => new Duration(TimeSpan.FromMilliseconds(milliseconds)));
    }

    [Fact]
    public void AcceptsOneMillisecondLength()
    {
        Assert.Null(Record.Exception(() => new Duration(TimeSpan.FromMilliseconds(1))));
    }
}
