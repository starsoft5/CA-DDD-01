
using Domain.Entities;

namespace Application.DTOs
{
    public class OrderItemCreateDto
    {
        public int Id { get; set; }
        public  Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
