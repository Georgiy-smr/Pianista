using System.ComponentModel.DataAnnotations;
using Pianista.Core.Songs;

namespace Pianista.Tests.Songs;

public sealed class StartTests
{
    [Fact]
    public void RejectsNegativeOffset()
    {
        Assert.Throws<ValidationException>(() => new Start(TimeSpan.FromMilliseconds(-1)));
    }

    [Fact]
    public void AcceptsZeroOffset()
    {
        Assert.Null(Record.Exception(() => new Start(TimeSpan.Zero)));
    }
}
