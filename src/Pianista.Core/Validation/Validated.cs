using System.ComponentModel.DataAnnotations;

namespace Pianista.Core.Validation;

public abstract record Validated
{
    protected Validated()
    {
        Validator.ValidateObject(this, new ValidationContext(this), validateAllProperties: true);
    }
}
