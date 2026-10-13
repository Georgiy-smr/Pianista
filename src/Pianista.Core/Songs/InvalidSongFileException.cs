namespace Pianista.Core.Songs;

public sealed class InvalidSongFileException(string path, Exception? inner = null)
    : PianistaException($"Song file '{path}' could not be read as a MIDI file.", inner)
{
    public string Path => string.Empty;
}
