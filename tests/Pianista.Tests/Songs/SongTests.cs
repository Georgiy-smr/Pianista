using Pianista.Core.Songs;

namespace Pianista.Tests.Songs;

public sealed class SongTests
{
    [Fact]
    public void OrdersNotesByStart()
    {
        Note first = new SampleNote(StartSeconds: 0).Note;
        Note second = new SampleNote(StartSeconds: 1).Note;
        Note third = new SampleNote(StartSeconds: 2).Note;

        Assert.Equal([first, second, third], new Song([], [third, first, second]).Notes);
    }

    [Fact]
    public void OrdersChordNotesByPitch()
    {
        Note root = new SampleNote(Pitch: 60).Note;
        Note third = new SampleNote(Pitch: 64).Note;
        Note fifth = new SampleNote(Pitch: 67).Note;

        Assert.Equal([root, third, fifth], new Song([], [fifth, third, root]).Notes);
    }
}
