using Application.DTOs;
using Domain.Entities;
using MediatR;

namespace Application.Queries.Users;

public class GetUserByIdQuery : IRequest<OrderReadDto>
{
    public int Id { get; }
    public GetUserByIdQuery(int id) => Id = id;
}
