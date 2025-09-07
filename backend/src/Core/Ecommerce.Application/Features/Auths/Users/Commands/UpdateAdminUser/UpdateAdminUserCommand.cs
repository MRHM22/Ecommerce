using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminUser;

public class UpdateAdminUserCommand : IRequest<Usuario>
{
    public string? Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Apellido { get; init; } = string.Empty;
    public string Telefono { get; init; } = string.Empty;
    public string? Role { get; init; }
}
