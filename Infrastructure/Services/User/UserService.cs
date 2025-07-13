//using Application.Interfaces;
using Application.Interfaces.Users;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.User;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public List<Domain.Entities.User> GetAll() => _context.Users.ToList();

    public Domain.Entities.User? GetById(int id) => _context.Users.FirstOrDefault(o => o.Id == id);

    public void Create(Domain.Entities.User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(Domain.Entities.User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = _context.Users.Find(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
    public Domain.Entities.User? GetByEmail(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }
}
