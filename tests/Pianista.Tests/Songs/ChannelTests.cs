using System.ComponentModel.DataAnnotations;
using Pianista.Core.Songs;

namespace Pianista.Tests.Songs;

public sealed class ChannelTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(16)]
    public void RejectsNumberOutsideMidiChannels(int number)
    {
        Assert.Throws<ValidationException>(() => new Channel(number));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(15)]
    public void AcceptsNumberAtMidiChannelBounds(int number)
    {
        Assert.Null(Record.Exception(() => new Channel(number)));
    }
}
