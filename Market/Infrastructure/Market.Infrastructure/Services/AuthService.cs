using Market.Application.Interfaces.Services;
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
}