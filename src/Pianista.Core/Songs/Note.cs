using System.ComponentModel.DataAnnotations;
using Pianista.Core.Validation;

namespace Pianista.Core.Songs;

public sealed record Note(
    Pitch Pitch,
    Start Start,
    Duration Duration,
    Velocity Velocity,
    [property: Range(0, int.MaxValue)] int Track,
    Channel Channel) : Validated
{
    public TimeSpan End => Start.Offset + Duration.Length;
}
