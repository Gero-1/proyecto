using MiradorB.Models;
using FluentValidation;
using MiradorB.Models;
using MiradorB.Models;

namespace MiradorB.Validaciones
{
    public class ValidacionRoles : AbstractValidator<Role>
    {
        public ValidacionRoles()
        {
            RuleFor(Role => Role.NomRol)
            .NotEmpty().WithMessage("El Nombre del rol es requerido.");

            RuleFor(Role => Role.Estado)
            .NotEmpty().WithMessage("El estado es requerido.");
        }

    }
}
