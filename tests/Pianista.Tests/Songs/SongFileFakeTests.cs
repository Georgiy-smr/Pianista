using Pianista.Core.Songs;

namespace Pianista.Tests.Songs;

public sealed class SongFileFakeTests
{
    [Fact]
    public async Task ParsesSongWithoutNotes()
    {
        Song song = await new ISongFile.Fake().Parsed("any.mid", TestContext.Current.CancellationToken);

        Assert.Empty(song.Notes);
    }
}
