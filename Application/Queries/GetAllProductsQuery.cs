using Application.DTOs;
using MediatR;

namespace Application.Queries;

public class GetAllProductsQuery : IRequest<List<ProductDTO>> { }
