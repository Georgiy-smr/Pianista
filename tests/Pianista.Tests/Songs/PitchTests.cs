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
}
