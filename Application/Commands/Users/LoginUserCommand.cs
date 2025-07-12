using MediatR;
using Application.DTOs.Users;

namespace Application.Command.Users;

public class LoginUserCommand : IRequest<UserLoginDto>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
