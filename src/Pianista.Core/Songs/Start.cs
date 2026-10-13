using Pianista.Core.Validation;

namespace Pianista.Core.Songs;

public sealed record Start([property: NotNegative] TimeSpan Offset) : Validated;
