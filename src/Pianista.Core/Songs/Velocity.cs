using System.ComponentModel.DataAnnotations;
using Pianista.Core.Validation;

namespace Pianista.Core.Songs;

public sealed record Velocity([property: Range(1, 127)] int Number) : Validated;
