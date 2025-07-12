// File: Application/Command/Users/CreateUserCommandHandler.cs

using Application.Command.Users;
using Application.DTOs.Users;
using Application.Interfaces.Users;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Users;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserReadDto>
{
    private readonly IUserService _userService;

    public CreateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public Task<UserReadDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.PasswordHash))
            throw new ArgumentException("Invalid user data");

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = dto.PasswordHash,
            Salt = dto.Salt
        };

        _userService.Create(user);

        var result = new UserReadDto
        {
            Id = user.Id,
            Email = user.Email,
            Password = user.PasswordHash,
            Salt = user.Salt // Fix for CS9035: Required member 'UserReadDto.Salt'  
        };

        return Task.FromResult(result);
    }
}
