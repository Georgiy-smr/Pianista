using System.ComponentModel.DataAnnotations;
using Pianista.Core.Validation;

namespace Pianista.Core.Songs;

public sealed record Channel([property: Range(0, 15)] int Number) : Validated;
