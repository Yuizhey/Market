using Market.Application.Interfaces.Services;
using Market.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Market.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
 
    public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<bool> RegisterAsync(string fullName, string password,string email, string confirmPassword)
    {
        var user = new IdentityUser { Email = email, UserName = fullName };
        var result = await _userManager.CreateAsync(user, password);
        await _userManager.AddToRoleAsync(user, UserRoles.CLientUser.ToString());

        if (!result.Succeeded)
            return false;
 
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
    
    public async Task<bool> AuthorRegisterAsync(string authorUserName, string password,string email)
    {
        var user = new IdentityUser { Email = email, UserName = authorUserName };
        var result = await _userManager.CreateAsync(user, password);
        await _userManager.AddToRoleAsync(user, UserRoles.AuthorUser.ToString());

        if (!result.Succeeded)
            return false;
 
        await _signInManager.SignInAsync(user, isPersistent: false);
        
        return true;
    }
    
    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}