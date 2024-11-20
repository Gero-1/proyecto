using MiradorB.Models;
using FluentValidation;
using MiradorB.Models;
using MiradorB.Models;

namespace MiradorB.Validaciones
{
    public class ValidacionClientes : AbstractValidator<Cliente>
    {
        public ValidacionClientes()
        {
            RuleFor(Cliente => Cliente.Nombres)
                .NotEmpty().WithMessage("El nombre es requerido.")
                .Length(3, 50).WithMessage("El nombre debe tener entre 3 y 50 caracteres.")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("El nombre solo debe contener letras y espacios.");


            RuleFor(Cliente => Cliente.Apellidos)
                .NotEmpty().WithMessage("El apellido es requerido.")
                .Length(3, 50).WithMessage("El apellido debe tener entre 3 y 50 caracteres.")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("El apellido solo debe contener letras y espacios.");

            RuleFor(Cliente => Cliente.Celular)
           .NotEmpty().WithMessage("El número de celular es requerido.")
           .Length(10, 15).WithMessage("El número de celular debe tener entre 10 y 15 dígitos.")
           .Matches(@"^\+?[0-9]+$").WithMessage("El número de celular solo debe contener números y puede incluir un prefijo internacional (+).");

            RuleFor(Cliente => Cliente.Correo)
            .NotEmpty().WithMessage("El correo electrónico es requerido.")
            .EmailAddress().WithMessage("El formato del correo electrónico no es válido.");

            RuleFor(Cliente => Cliente.Contrasena)
            .NotEmpty().WithMessage("La contraseña es requerida.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .MaximumLength(100).WithMessage("La contraseña no debe exceder los 100 caracteres.");



        }
    }
}
