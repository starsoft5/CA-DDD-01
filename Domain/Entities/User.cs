using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string Salt { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
