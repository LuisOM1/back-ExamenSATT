using FluentValidation;
using ExamenSATT.DTOs;

namespace ExamenSATT.Validators;

// Clase validador que define reglas para el DTO
public class EmpleadoValidator : AbstractValidator<EmpleadoCreateUpdateDTO>
{
    public EmpleadoValidator()
    {
        RuleFor(x => x.IdArea)
            .NotEmpty().WithMessage("El área es obligatoria.")
            .GreaterThan(0).WithMessage("El ID de área debe ser mayor a 0.");

        RuleFor(x => x.NombEmpl)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(50).WithMessage("El nombre no puede exceder los 50 caracteres.");

        RuleFor(x => x.ApeEmpl)
            .NotEmpty().WithMessage("El apellido es requerido.")
            .MaximumLength(50).WithMessage("El apellido no puede exceder los 50 caracteres.");

        RuleFor(x => x.EmaiEmpl)
            .NotEmpty().WithMessage("El correo es obligatorio.")
            .EmailAddress().WithMessage("El formato del correo no es válido.");

        RuleFor(x => x.SuelEmpl)
            .GreaterThan(0).WithMessage("El sueldo debe ser una cantidad positiva.");

        RuleFor(x => x.FechIngr)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de ingreso no puede ser futura.");
    }
}