using Pianista.Core.Validation;

namespace Pianista.Core.Songs;

public sealed record Note(
    Pitch Pitch,
    Start Start,
    Duration Duration,
    Velocity Velocity,
    int Track,
    Channel Channel) : Validated;
