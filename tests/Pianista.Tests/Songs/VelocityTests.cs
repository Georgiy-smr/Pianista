using System.ComponentModel.DataAnnotations;
using Pianista.Core.Songs;

namespace Pianista.Tests.Songs;

public sealed class VelocityTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(128)]
    public void RejectsNumberOutsideAudibleMidiRange(int number)
    {
        Assert.Throws<ValidationException>(() => new Velocity(number));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(127)]
    public void AcceptsNumberAtAudibleMidiRangeBounds(int number)
    {
        Assert.Null(Record.Exception(() => new Velocity(number)));
    }
}
