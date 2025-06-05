using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using Market.Domain.Entities;
using Market.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Market.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserDescriptionRepository _userDescriptionRepository;
    private readonly IAuthorUserDescriptionRepository _authorUserDescriptionRepository;
 
    public AuthService(
        UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager,
        IUserDescriptionRepository userDescriptionRepository,
        IAuthorUserDescriptionRepository authorUserDescriptionRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userDescriptionRepository = userDescriptionRepository;
        _authorUserDescriptionRepository = authorUserDescriptionRepository;
    }
    
    public async Task<bool> RegisterAsync(string userName, string password, string email, string confirmPassword)
    {
        var user = new IdentityUser { Email = email, UserName = userName };
        var result = await _userManager.CreateAsync(user, password);
        await _userManager.AddToRoleAsync(user, UserRoles.CLientUser.ToString());

        if (!result.Succeeded)
            return false;

        var userDescription = new UserDescription
        {
            Id = Guid.NewGuid(),
            IdentityUserId = Guid.Parse(user.Id),
        };

        await _userDescriptionRepository.AddAsync(userDescription);
        await _signInManager.SignInAsync(user, isPersistent: false);
        
        return true;
    }
 
    public async Task<bool> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return false;
        
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
        };

        await _authorUserDescriptionRepository.AddAsync(authorUserDescription);
        await _signInManager.SignInAsync(user, isPersistent: false);
        
        return true;
    }
    
    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}