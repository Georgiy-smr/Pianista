using Pianista.Core.Validation;

namespace Pianista.Core.Songs;

public sealed record Duration(TimeSpan Length) : Validated;
