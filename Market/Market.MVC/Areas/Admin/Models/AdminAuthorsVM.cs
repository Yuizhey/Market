using Market.Application.Features.Authors.Queries.GetAllAuthors;

namespace Market.MVC.Areas.Admin.Models;

public class AdminAuthorsVM
{
    public IEnumerable<GetAllAuthorsResponse> Authors { get; set; } = new List<GetAllAuthorsResponse>();
} 