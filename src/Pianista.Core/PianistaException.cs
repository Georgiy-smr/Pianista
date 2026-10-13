namespace Pianista.Core;

public abstract class PianistaException : Exception
{
    protected PianistaException(string message)
        : base(message)
    {
    }

    protected PianistaException(string message, Exception? inner)
        : base(message, inner)
    {
    }
}
