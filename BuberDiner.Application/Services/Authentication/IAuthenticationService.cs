using BuberDiner.Application.Common.Errors;
using OneOf;

namespace BuberDiner.Application.Services.Authentication;

public interface IAuthenticationService
{
    OneOf<AuthenticationResult, DuplicateEmailError> Login(string email, string password);

    OneOf<AuthenticationResult, DuplicateEmailError> Register(string firstName, string lastName, string email, string password);
}