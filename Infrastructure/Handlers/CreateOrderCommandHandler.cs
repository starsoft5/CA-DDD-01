using Application.Commands;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderReadDto>
{
    private readonly AppDbContext _context;

    public CreateOrderCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderReadDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var order = new Order
        {
            CustomerName = dto.CustomerName,
            OrderDate = dto.OrderDate,
            Items = dto.Items.Select(i =>
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == i.Product.Id);
                if (product == null)
                {
                    throw new InvalidOperationException($"Product with ID {i.Product.Id} not found.");
                }

                return new OrderItem
                {
                    Product = product,
                    Quantity = i.Quantity,
                    Price = i.Price
                };
            }).ToList()
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        return new OrderReadDto
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            OrderDate = order.OrderDate.ToString("yyyy-MM-dd"),
            Items = order.Items.Select(i => new OrderItemReadDto
            {
                Id = i.Id,
                Product = i.Product,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };
    }
}