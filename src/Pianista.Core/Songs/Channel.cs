using Pianista.Core.Validation;

namespace Pianista.Core.Songs;

public sealed record Channel(int Number) : Validated;
