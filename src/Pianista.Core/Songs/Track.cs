using System.ComponentModel.DataAnnotations;
using Pianista.Core.Validation;

namespace Pianista.Core.Songs;

public sealed record Track([property: Range(0, int.MaxValue)] int Index, string Name) : Validated;
