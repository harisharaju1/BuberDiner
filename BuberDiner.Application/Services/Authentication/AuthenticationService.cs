using BuberDiner.Application.Common.Interfaces.Authentication;
using BuberDiner.Application.Common.Interfaces.Persistence;
using BuberDiner.Domain.Common.Errors;
using BuberDiner.Domain.Entities;
using ErrorOr;

namespace BuberDiner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    /// <summary>
    /// Represents object for JWT token generation.
    /// </summary>
    public readonly IJwtTokenGenerator _jwtTokenGenerator;

    public readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public ErrorOr<AuthenticationResult> Login(string email, string password)
    {
        // make sure user does exist
        if (_userRepository.GetUserByEmail(email) is not User user)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        // validate the password is correct
        if (user.Password != password)
        {
            return new[] { Errors.Authentication.InvalidCredentials };
        }

        // create JWT Token

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token);
    }

    public ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
    {
        // check if user already exists
        if (_userRepository.GetUserByEmail(email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        // create user (and generate unique Id) & persist to DB
        var user = new User
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Password = password
        };

        _userRepository.Add(user);

        // generate JWT Token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}