using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using Market.Domain.Entities;
using Market.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Market.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserDescriptionRepository _userDescriptionRepository;
    private readonly IAuthorUserDescriptionRepository _authorUserDescriptionRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager,
        IUserDescriptionRepository userDescriptionRepository,
        IAuthorUserDescriptionRepository authorUserDescriptionRepository,
        IEmailService emailService,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userDescriptionRepository = userDescriptionRepository;
        _authorUserDescriptionRepository = authorUserDescriptionRepository;
        _emailService = emailService;
        _logger = logger;
    }
    
    public async Task<bool> RegisterAsync(string userName, string password, string email, string confirmPassword)
    {
        _logger.LogInformation("Попытка регистрации нового пользователя: {Email}, {Username}", email, userName);
        
        var user = new IdentityUser { Email = email, UserName = userName };
        var result = await _userManager.CreateAsync(user, password);
        await _userManager.AddToRoleAsync(user, UserRoles.CLientUser.ToString());

        if (!result.Succeeded)
        {
            _logger.LogWarning("Ошибка при регистрации пользователя {Email}: {Errors}", 
                email, string.Join(", ", result.Errors.Select(e => e.Description)));
            return false;
        }

        _logger.LogInformation("Пользователь успешно зарегистрирован: {Email}", email);

        var userDescription = new UserDescription
        {
            Id = Guid.NewGuid(),
            IdentityUserId = Guid.Parse(user.Id),
            Email = user.Email,
        };

        await _userDescriptionRepository.AddAsync(userDescription);
        await _signInManager.SignInAsync(user, isPersistent: false);
        
        var subject = "Добро пожаловать в Market!";
        var message = $"Здравствуйте, {userName}!\n\n" +
                     "Спасибо за регистрацию в нашем магазине.\n" +
                     $"Ваш логин: {email}\n" +
                     $"Ваш пароль: {password}\n\n" +
                     "С уважением,\nКоманда Market";
        
        await _emailService.SendEmailAsync(email, subject, message);
        
        return true;
    }
 
    public async Task<bool> LoginAsync(string email, string password)
    {
        _logger.LogInformation("Попытка входа пользователя: {Email}", email);
        
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            _logger.LogWarning("Пользователь не найден: {Email}", email);
            return false;
        }
        
        await _signInManager.PasswordSignInAsync(user, password, false, false);
        
        return true;
    }
    
    public async Task<bool> AuthorRegisterAsync(string authorUserName, string password, string email)
    {
        var user = new IdentityUser { Email = email, UserName = authorUserName };
        var result = await _userManager.CreateAsync(user, password);
        await _userManager.AddToRoleAsync(user, UserRoles.AuthorUser.ToString());

        if (!result.Succeeded)
            return false;

        var authorUserDescription = new AuthorUserDescription
        {
            Id = Guid.NewGuid(),
            IdentityUserId = Guid.Parse(user.Id),
            Email = user.Email,
        };

        await _authorUserDescriptionRepository.AddAsync(authorUserDescription);
        await _signInManager.SignInAsync(user, isPersistent: false);
        
        var subject = "Добро пожаловать в Market как автор!";
        var message = $"Здравствуйте, {authorUserName}!\n\n" +
                     "Спасибо за регистрацию в качестве автора в нашем магазине.\n" +
                     $"Ваш логин: {email}\n" +
                     $"Ваш пароль: {password}\n\n" +
                     "С уважением,\nКоманда Market";
        
        await _emailService.SendEmailAsync(email, subject, message);
        
        return true;
    }
    
    public async Task LogoutAsync()
    {
        _logger.LogInformation("Выход пользователя из системы");
        await _signInManager.SignOutAsync();
    }

    public async Task<bool> AdminLoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return false;

        var isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin.ToString());
        if (!isAdmin)
            return false;
        
        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
        return result.Succeeded;
    }
}