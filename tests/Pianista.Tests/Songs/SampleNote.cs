using Pianista.Core.Songs;

namespace Pianista.Tests.Songs;

internal sealed record SampleNote(
    double StartSeconds = 0,
    double DurationSeconds = 1,
    int Pitch = 60,
    int Track = 0)
{
    public Note Note => new(
        new Pitch(Pitch),
        new Start(TimeSpan.FromSeconds(StartSeconds)),
        new Duration(TimeSpan.FromSeconds(DurationSeconds)),
        new Velocity(100),
        Track,
        new Channel(0));
}
