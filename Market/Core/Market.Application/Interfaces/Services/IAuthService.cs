namespace Market.Application.Interfaces.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(string userName, string password, string email, string confirmPassword);
    Task<bool> LoginAsync(string email, string password);
    Task<bool> AuthorRegisterAsync(string authorUserName, string password, string email);
    Task LogoutAsync();
    Task<bool> AdminLoginAsync(string email, string password);
}