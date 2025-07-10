using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = "";
        [MaxLength(500)]
        public string Description { get; set; } = "";
        public decimal Price { get; set; } = 0;
    }
}
