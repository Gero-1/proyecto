using MiradorB.Models;
using FluentValidation;
using MiradorB.Models;
using MiradorB.Models;

namespace MiradorB.Validaciones
{
    public class ValidacionTipoDocumento : AbstractValidator<TipoDocumento>
    {
        public ValidacionTipoDocumento()
        {

            RuleFor(TipoDocumento => TipoDocumento.NomTipoDcumento)
            .NotEmpty().WithMessage("El tipo de documento es requerido.");
        }
    }
}
