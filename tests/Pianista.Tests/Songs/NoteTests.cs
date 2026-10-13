using System.ComponentModel.DataAnnotations;
using Pianista.Core.Songs;

namespace Pianista.Tests.Songs;

public sealed class NoteTests
{
    [Fact]
    public void RejectsNegativeTrack()
    {
        Assert.Throws<ValidationException>(() => new Note(
            new Pitch(60),
            new Start(TimeSpan.Zero),
            new Duration(TimeSpan.FromSeconds(1)),
            new Velocity(100),
            -1,
            new Channel(0)));
    }
}
