using Application.DTOs.Users;
using MediatR;

namespace Application.Queries.Users;

public class GetAllUsersQuery : IRequest<List<UserReadDto>> { }
