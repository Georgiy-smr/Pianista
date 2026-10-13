using System.Diagnostics;

namespace Pianista.Core.Songs;

public interface ISongFile
{
    ValueTask<Song> Parsed(string path, CancellationToken cancellationToken);

    public class Fake : ISongFile
    {
        public ValueTask<Song> Parsed(string path, CancellationToken cancellationToken)
        {
            Trace.WriteLine($"{nameof(Fake)}.{nameof(Parsed)} called with {path}");
            return ValueTask.FromResult(new Song([], []));
        }
    }
}
