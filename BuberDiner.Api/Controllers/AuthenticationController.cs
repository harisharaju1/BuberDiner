using BuberDiner.Application.Common.Errors;
using BuberDiner.Application.Services.Authentication;
using BuberDiner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace BuberDiner.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        OneOf<AuthenticationResult, DuplicateEmailError> registerResult = _authenticationService.Register(
            request.FirstName, request.LastName, request.Email, request.Password);

        return registerResult.Match(authResult => Ok(MapAuthResult(authResult)),
        _ => Problem(statusCode: StatusCodes.Status409Conflict, title: "Email already exists."));

        // if (registerResult.IsT0)
        // {
        //     var authResult = registerResult.AsT0;
        //     AuthenticationResponse response = MapAuthResult(authResult);

        //     return Ok(response);
        // }

        // return Problem(statusCode: StatusCodes.Status409Conflict, title: "Email already exists.");
    }

    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
                        authResult.user.Id,
                        authResult.user.FirstName,
                        authResult.user.LastName,
                        authResult.user.Email,
                        authResult.Token
                    );
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        OneOf<AuthenticationResult, DuplicateEmailError> loginResult = _authenticationService.Login(request.Email, request.Password);

        if (loginResult.IsT0)
        {
            var authResult = loginResult.AsT0;

            var response = new AuthenticationResponse(
                authResult.user.Id,
                authResult.user.FirstName,
                authResult.user.LastName,
                authResult.user.Email,
                authResult.Token
            );

            return Ok(response);
        }

        return Problem(statusCode: StatusCodes.Status409Conflict, title: "Email does not exist.");
    }
}