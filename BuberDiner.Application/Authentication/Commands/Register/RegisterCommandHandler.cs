using BuberDiner.Application.Common.Interfaces.Authentication;
using BuberDiner.Application.Common.Interfaces.Persistence;
using BuberDiner.Domain.Entities;
using BuberDiner.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using BuberDiner.Application.Authentication.Common;

namespace BuberDiner.Application.Authentication.Commands.Register;

public class LoginQueryHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    public readonly IJwtTokenGenerator _jwtTokenGenerator;
    public readonly IUserRepository _userRepository;

    public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }


    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        // check if user already exists
        if (_userRepository.GetUserByEmail(command.Email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        // create user (and generate unique Id) & persist to DB
        var user = new User
        {
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Password = command.Password
        };

        _userRepository.Add(user);

        // generate JWT Token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}