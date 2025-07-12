using MediatR;
using Application.DTOs;
using Application.DTOs.Users;

namespace Application.Command.Users;

public class CreateUserCommand : IRequest<UserReadDto>
{
    public UserCreateDto Dto { get; }
    public CreateUserCommand(UserCreateDto dto) => Dto = dto;

    public CreateUserCommand()
    {
    }
}