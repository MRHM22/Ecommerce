using FluentValidation;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminUser;

public class UpdateAdminUserCommandValidator : AbstractValidator<UpdateAdminUserCommand>
{
    public UpdateAdminUserCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es requerido");
        RuleFor(x => x.Apellido).NotEmpty().WithMessage("El apellido es requerido");
        RuleFor(x => x.Telefono).NotEmpty().WithMessage("El Telefono es requerido");
    }
}