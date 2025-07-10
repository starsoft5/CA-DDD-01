using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProductDTO
    {
        [MaxLength(100)]
        public string Name { get; set; } = "";
        [MaxLength(500)]
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
    }
}
