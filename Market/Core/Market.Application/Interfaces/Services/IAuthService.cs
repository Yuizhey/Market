namespace Market.Application.Interfaces.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(string fullName, string password, string email, string confirmPassword);
}