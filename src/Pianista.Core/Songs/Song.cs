namespace Pianista.Core.Songs;

public sealed class Song
{
    public Song(IEnumerable<Track> tracks, IEnumerable<Note> notes)
    {
        Tracks = [.. tracks];
        Notes = [.. notes.OrderBy(note => note.Start.Offset).ThenBy(note => note.Pitch.Number)];
        Length = Notes.Select(note => note.End).DefaultIfEmpty(TimeSpan.Zero).Max();
    }

    public IReadOnlyList<Track> Tracks { get; }

    public IReadOnlyList<Note> Notes { get; }

    public TimeSpan Length { get; }
}
