using BuberDiner.Domain.Entities;

namespace BuberDiner.Application.Services.Authentication;

public record AuthenticationResult(
    User user,
    string Token
);