using System.Data;
using Ecommerce.Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio");
        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("El apellido es obligatorio");
   }
}
