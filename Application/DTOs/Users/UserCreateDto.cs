using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Users;
public class UserCreateDto
{
    public  string Email { get; set; }
    public  string Password { get; set; }
    public  string ConfirmPassword { get; set; }
    public  string PasswordHash { get; set; }
    public  string Salt { get; set; }
}
