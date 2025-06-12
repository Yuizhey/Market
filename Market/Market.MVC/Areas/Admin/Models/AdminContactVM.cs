using Market.Application.Features.Contact.Queries.GetContactRequests;
using Market.Domain.Entities;

namespace Market.MVC.Areas.Admin.Models;

public class AdminContactVM
{
    public IEnumerable<GetContactRequestsResponse> Responses { get; set; } = new List<GetContactRequestsResponse>();
}