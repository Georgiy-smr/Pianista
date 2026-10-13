using System.ComponentModel.DataAnnotations;

namespace Pianista.Tests.Songs;

public sealed class NoteTests
{
    [Fact]
    public void RejectsNegativeTrack()
    {
        Assert.Throws<ValidationException>(() => new SampleNote(Track: -1).Note);
    }

    [Fact]
    public void EndsAfterItsDurationFromStart()
    {
        Assert.Equal(
            TimeSpan.FromSeconds(1.5),
            new SampleNote(StartSeconds: 1, DurationSeconds: 0.5).Note.End);
    }
}
