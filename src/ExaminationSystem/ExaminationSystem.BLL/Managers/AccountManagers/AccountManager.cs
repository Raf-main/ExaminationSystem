using System.Security.Claims;
using AutoMapper;
using ExaminationSystem.BLL.Exceptions;
using ExaminationSystem.BLL.Helpers;
using ExaminationSystem.BLL.Managers.AccountManagers.Exceptions;
using ExaminationSystem.BLL.Managers.EmailManagers;
using ExaminationSystem.BLL.Managers.EmailManagers.Configuration;
using ExaminationSystem.BLL.Managers.TokenManagers.Interfaces;
using ExaminationSystem.BLL.Models;
using ExaminationSystem.BLL.Models.Create;
using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ExaminationSystem.BLL.Managers.AccountManagers;

public class AccountManager : IAccountManager
{
    private readonly IEmailManager _emailManager;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly HttpContext _httpContext;
    private readonly IMapper _mapper;
    private readonly ITemplateManager _templateManager;
    private readonly ITokenManager _tokenManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UrlHelper _urlHelper;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountManager(UserManager<ApplicationUser> userManager,
        ITokenManager tokenManager,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ITemplateManager templateManager,
        IEmailManager emailManager,
        IHttpContextAccessor httpContextAccessor,
        IDateTimeProvider dateTimeProvider,
        UrlHelper urlHelper)
    {
        _userManager = userManager;
        _tokenManager = tokenManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _templateManager = templateManager;
        _emailManager = emailManager;
        _dateTimeProvider = dateTimeProvider;
        _urlHelper = urlHelper;
        _httpContext = httpContextAccessor.HttpContext;
    }

    public async Task<LoginResult> LoginAsync(UserLoginData userData)
    {
        var user = await _userManager.FindByEmailAsync(userData.Email);

        if (user == null)
        {
            throw new NotFoundException(
                $"User with email {userData.Email} was not found");
        }

        if (!user.EmailConfirmed)
        {
            throw new PermissionDeniedException("Confirm your registration first");
        }

        if (!await _userManager.CheckPasswordAsync(user, userData.Password))
        {
            throw new PermissionDeniedException($"Incorrect password for user with email {userData.Password}");
        }

        await DeleteExpiredRefreshTokensAsync(user);

        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        if (userRoles != null)
        {
            authClaims.AddRange(userRoles.Where(role => !string.IsNullOrEmpty(role))
                .Select(role => new Claim(ClaimTypes.Role, role)));
        }

        var accessToken = _tokenManager.AccessTokenGenerator.GenerateAccessToken(authClaims);
        var refreshToken = _tokenManager.RefreshTokenGenerator.GenerateUserRefreshToken(user);
        var refreshTokenDbEntry = _mapper.Map<RefreshTokenDbEntry>(refreshToken);

        await _unitOfWork.RefreshTokens.CreateAsync(refreshTokenDbEntry);
        await _unitOfWork.SaveChangesAsync();

        var userTokens = new UserTokens
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpirationDate = refreshToken.UtcExpirationDate
        };

        return new LoginResult
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.UserName,
            Tokens = userTokens
        };
    }

    public async Task RegisterAsync(UserCreateData userData, string emailConfirmationAction)
    {
        ApplicationUser? user;

        if ((user = await _userManager.FindByEmailAsync(userData.Email)) != null)
        {
            if (user.EmailConfirmed)
            {
                throw new UserAlreadyExistsException($"User with email {userData.Email} already exists");
            }

            await SendEmailConfirmationMessageAsync(user, emailConfirmationAction);

            return;
        }

        user = new ApplicationUser
        {
            UserName = userData.Name,
            Email = userData.Email
        };

        var result = await _userManager.CreateAsync(user, userData.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);

            throw new ValidationException($"Something went wrong while creating user with email {userData.Email}",
                errors);
        }

        await _unitOfWork.SaveChangesAsync();
        await SendEmailConfirmationMessageAsync(user, emailConfirmationAction);
    }

    public async Task<UserTokens> RefreshTokenAsync(UserTokens tokens)
    {
        var principal = _tokenManager.GetPrincipalFromExpiredToken(tokens.AccessToken);

        if (principal == null)
        {
            throw new SecurityTokenException("Invalid token");
        }

        var emailClaim = principal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);

        if (emailClaim == null)
        {
            throw new SecurityTokenException("Invalid token");
        }

        var user = await _userManager.FindByEmailAsync(emailClaim.Value);

        if (user == null)
        {
            throw new NotFoundException($"User with email {emailClaim.Value} was not found");
        }

        var userRefreshTokens = await _unitOfWork.RefreshTokens.GetByUserIdAsync(user.Id);
        var userRefreshToken = userRefreshTokens.FirstOrDefault(t => t.Token == tokens.RefreshToken);

        if (userRefreshToken == null)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        if (_dateTimeProvider.GetUtcNow() > userRefreshToken.UtcExpirationTime)
        {
            throw new SecurityTokenExpiredException("Refresh token is expired");
        }

        await DeleteExpiredRefreshTokensAsync(user);

        var newAccessToken = _tokenManager.AccessTokenGenerator.GenerateAccessToken(principal.Claims);
        await _unitOfWork.SaveChangesAsync();

        return new UserTokens
        {
            AccessToken = newAccessToken,
            RefreshToken = userRefreshToken.Token,
            RefreshTokenExpirationDate = userRefreshToken.UtcExpirationTime
        };
    }

    public async Task ConfirmEmailAsync(ConfirmEmail confirmEmailData)
    {
        var user = await _userManager.FindByEmailAsync(confirmEmailData.Email);

        if (user == null)
        {
            throw new NotFoundException($"User with email {confirmEmailData.Email} was not found");
        }

        var result = await _userManager.ConfirmEmailAsync(user, confirmEmailData.Token);

        if (!result.Succeeded)
        {
            throw new ValidationException("Token is not valid", result.Errors.Select(e => e.Description));
        }
    }

    public async Task<ApplicationUser?> FindUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<ApplicationUser?> FindUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public ClaimsPrincipal? GetCurrentUser()
    {
        return _httpContext.User;
    }

    public string? GetCurrentUserClaim(string claimKey)
    {
        var user = _httpContext.User;
        var claim = user?.Claims.FirstOrDefault(c => c.Type == claimKey);

        return claim?.Value;
    }

    private async Task SendEmailConfirmationMessageAsync(ApplicationUser user, string emailConfirmationAction)
    {
        var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        IEnumerable<string> emails = new List<string>
        {
            user.Email
        };

        var emailTemplate = await _templateManager.GetTemplateAsync(TemplateNames.EmailConfirmation);
        var defaultServerUrl = _urlHelper.ServerUrls.FirstOrDefault();

        IDictionary<string, string> properties = new Dictionary<string, string>
        {
            { "BaseURL", defaultServerUrl! },
            { "Action", emailConfirmationAction },
            { "tokenValue", emailConfirmationToken },
            { "emailValue", user.Email }
        };

        emailTemplate = _templateManager.ReplaceTemplateProperty(emailTemplate, properties);
        var email = new EmailMessage(emails, "Registration", emailTemplate);
        await _emailManager.SendEmailAsync(email);
    }

    private async Task DeleteExpiredRefreshTokensAsync(ApplicationUser user)
    {
        await _unitOfWork.RefreshTokens.DeleteRangeAsync(t =>
            t.UserId == user.Id && _dateTimeProvider.GetUtcNow() > t.UtcExpirationTime);
    }
}