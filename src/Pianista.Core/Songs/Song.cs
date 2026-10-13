namespace Pianista.Core.Songs;

public sealed class Song(IEnumerable<Track> tracks, IEnumerable<Note> notes)
{
    public IReadOnlyList<Track> Tracks { get; } = [.. tracks];

    public IReadOnlyList<Note> Notes { get; } =
    [
        .. notes
            .OrderBy(note => note.Start.Offset)
            .ThenBy(note => note.Pitch.Number)
    ];

    public TimeSpan Length => Notes.Select(note => note.End).DefaultIfEmpty(TimeSpan.Zero).Max();
}
