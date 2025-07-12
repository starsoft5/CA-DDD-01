﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Users;
public class UserReadDto
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public  string Password { get; set; }
    public  string Salt { get; set; }
}
