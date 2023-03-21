using BuberDiner.Domain.Entities;

namespace BuberDiner.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    User? GetUserByEmail(string email);
    void Add(User user);
}