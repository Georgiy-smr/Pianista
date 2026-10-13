using System.ComponentModel.DataAnnotations;
using Pianista.Core.Validation;

namespace Pianista.Core.Songs;

public sealed record Pitch([property: Range(0, 127)] int Number) : Validated;
