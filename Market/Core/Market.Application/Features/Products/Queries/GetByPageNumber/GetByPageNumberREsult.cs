namespace Market.Application.Features.Products.Queries.GetByPageNumber;

public class GetByPageNumberREsult
{
    public IEnumerable<GetByPageNumberDto> Products { get; set; } = [];
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
}