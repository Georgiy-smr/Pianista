using System.ComponentModel.DataAnnotations;
using Pianista.Core.Songs;

namespace Pianista.Tests.Songs;

public sealed class PitchTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(128)]
    public void RejectsNumberOutsideMidiRange(int number)
    {
        Assert.Throws<ValidationException>(() => new Pitch(number));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(127)]
    public void AcceptsNumberAtMidiRangeBounds(int number)
    {
        Assert.Null(Record.Exception(() => new Pitch(number)));
    }
}
