namespace Pianista.Core.Songs;

public interface ISongFile
{
    ValueTask<Song> Parsed(string path, CancellationToken cancellationToken);

    public class Fake : ISongFile
    {
        public ValueTask<Song> Parsed(string path, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
