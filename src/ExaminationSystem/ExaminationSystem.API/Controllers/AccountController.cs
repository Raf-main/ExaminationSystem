using AutoMapper;
using ExaminationSystem.API.DTOs.Request;
using ExaminationSystem.API.DTOs.Response;
using ExaminationSystem.BLL.Exceptions;
using ExaminationSystem.BLL.Managers.AccountManagers;
using ExaminationSystem.BLL.Managers.AccountManagers.Exceptions;
using ExaminationSystem.BLL.Models;
using ExaminationSystem.BLL.Models.Create;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ExaminationSystem.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private const string RefreshTokenCookieKey = "RefreshToken";
    private readonly IAccountManager _accountManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IMapper _mapper;

    public AccountController(IAccountManager accountManager,
        IMapper mapper,
        ILogger<AccountController> logger)
    {
        _accountManager = accountManager;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest loginRequestData)
    {
        LoginResult loginResult;

        try
        {
            loginResult = await _accountManager.LoginAsync(_mapper.Map<UserLoginData>(loginRequestData));
            var tokens = _mapper.Map<TokenResponse>(loginResult);
            SetResponseCookie(RefreshTokenCookieKey, tokens.RefreshToken, tokens.RefreshTokenExpirationDate,
                true);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, $"User with email {loginRequestData.Email} was not found");

            return NotFound(ex.Message);
        }
        catch (PermissionDeniedException ex)
        {
            _logger.LogError(ex, $"Permissions denied for user with email {loginRequestData.Email}");

            return Unauthorized(ex.Message);
        }

        var loginResponse = _mapper.Map<LoginResponse>(loginResult);

        return Ok(loginResponse);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest registerRequestData)
    {
        var emailConfirmationAction = Url.Action("ConfirmEmail", "Account");

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.Root.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            await _accountManager.RegisterAsync(_mapper.Map<UserCreateData>(registerRequestData),
                emailConfirmationAction!);
        }
        catch (UserAlreadyExistsException ex)
        {
            _logger.LogError(ex, $"User with email {registerRequestData.Email} already exists");

            return Conflict(ex.Message); // mb not suitable status code
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, $"Validation errors during user registration");

            return BadRequest(ex.ValidationErrors);
        }

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshAccessRequest refreshAccessRequestData)
    {
        TokenResponse tokenResponse;

        if (!Request.Cookies.TryGetValue(RefreshTokenCookieKey, out var refreshToken) ||
            string.IsNullOrEmpty(refreshToken))
        {

            _logger.LogInformation("Request doesn't contain refresh token");

            return Unauthorized("Request doesn't contain refresh token");
        }

        try
        {
            var tokens = _mapper.Map<UserTokens>(refreshAccessRequestData);
            tokens.RefreshToken = refreshToken;

            var newTokens = await _accountManager.RefreshTokenAsync(tokens);
            tokenResponse = _mapper.Map<TokenResponse>(newTokens);
        }
        catch (SecurityTokenExpiredException ex)
        {
            _logger.LogError(ex, $"Refresh token {refreshToken} is expired");

            return Unauthorized(ex.Message);
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest(ex.Message);
        }

        SetResponseCookie(RefreshTokenCookieKey, tokenResponse.RefreshToken, tokenResponse.RefreshTokenExpirationDate,
            true);

        return Ok(tokenResponse.AccessToken);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmEmailAsync([FromForm] ConfirmAccountRequest confirmAccountRequest)
    {
        var confirmationData = new ConfirmEmail
        {
            Email = confirmAccountRequest.Email,
            Token = confirmAccountRequest.Token
        };

        try
        {
            await _accountManager.ConfirmEmailAsync(confirmationData);

            return Ok("Your email was successfully confirmed");
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, $"User with email {confirmationData.Email} was not found");

            return NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex,
                $"Validation errors during email confirmation for user with email {confirmAccountRequest.Email}");

            return BadRequest(ex.ValidationErrors);
        }
    }

    [NonAction]
    private void SetResponseCookie(string key, string value, DateTime expirationDate, bool httpOnly = false)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = httpOnly,
            Expires = expirationDate
        };

        HttpContext.Response.Cookies.Append(key, value, cookieOptions);
    }
}