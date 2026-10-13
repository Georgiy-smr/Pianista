using System.ComponentModel.DataAnnotations;
using Pianista.Core.Songs;

namespace Pianista.Tests.Songs;

public sealed class TrackTests
{
    [Fact]
    public void RejectsNegativeIndex()
    {
        Assert.Throws<ValidationException>(() => new Track(-1, "Piano"));
    }

    [Fact]
    public void AcceptsFirstIndexWithEmptyName()
    {
        Assert.Null(Record.Exception(() => new Track(0, string.Empty)));
    }
}
