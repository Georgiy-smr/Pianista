using Pianista.Core.Songs;

namespace Pianista.Tests.Songs;

public sealed class InvalidSongFileExceptionTests
{
    [Fact]
    public void ExposesTheGivenPath()
    {
        Assert.Equal("songs/broken.mid", new InvalidSongFileException("songs/broken.mid").Path);
    }
}
