using Pianista.Core.Validation;

namespace Pianista.Core.Songs;

public sealed record Start(TimeSpan Offset) : Validated;
