using Market.Application.Features.Users.Queries.GetAllUsers;

namespace Market.MVC.Areas.Admin.Models;

public class AdminUsersVM
{
    public IEnumerable<GetAllUsersResponse> Users { get; set; } = new List<GetAllUsersResponse>();
} 