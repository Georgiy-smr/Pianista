using System.ComponentModel.DataAnnotations;

namespace Pianista.Core.Validation;

[AttributeUsage(AttributeTargets.Property)]
public sealed class PositiveAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return value is TimeSpan span && span > TimeSpan.Zero;
    }
}
