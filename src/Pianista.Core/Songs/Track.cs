using Pianista.Core.Validation;

namespace Pianista.Core.Songs;

public sealed record Track(int Index, string Name) : Validated;
