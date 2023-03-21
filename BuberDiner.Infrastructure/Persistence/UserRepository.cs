using BuberDiner.Application.Common.Interfaces.Persistence;
using BuberDiner.Domain.Entities;

namespace BuberDiner.Infrastructure.Persistence;

public class userRepository : IUserRepository
{
    private static readonly List<User> _users = new();
    public void Add(User user)
    {
        _users.Add(user);
    }

    public User? GetUserByEmail(string email)
    {
        return _users.SingleOrDefault(user => user.Email == email);
    }
}