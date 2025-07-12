﻿using Domain.Entities;

namespace Application.Interfaces.Users;

public interface IUserService
{
    List<User> GetAll();
    User? GetById(int id);
    void Create(User user);
    void Update(User user);
    void Delete(int id);
    User? GetByEmail(string email);
}
