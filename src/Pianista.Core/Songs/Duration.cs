using Pianista.Core.Validation;

namespace Pianista.Core.Songs;

public sealed record Duration([property: Positive] TimeSpan Length) : Validated;
