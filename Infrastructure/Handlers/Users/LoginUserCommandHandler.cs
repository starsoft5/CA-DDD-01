using Application.Command.Users;
using Application.DTOs.Users;
using Application.Interfaces.Users;
using Infrastructure.Security;
using MediatR;

namespace Infrastructure.Handlers.Users
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserLoginDto>
    {
        private readonly IUserService _userService;

        public LoginUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public Task<UserLoginDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = _userService.GetByEmail(request.Email); // Get user from DB

            if (user == null)
            {
                return Task.FromResult(new UserLoginDto
                {
                    Email = request.Email 
                });
            }

            var hasher = new PasswordHasher();
            var valid = hasher.Verify(request.Password, user.PasswordHash, user.Salt);

            return Task.FromResult(new UserLoginDto
            {
                Email = user.Email,
                IsAuthenticated = valid
            });
        }
    }
}
