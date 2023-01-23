namespace Catalog.Host.Models.Requests;

public class PaginatedItemsByBrandRequest
{
    public string Brand { get; set; } = null!;

    public int PageIndex { get; set; }

    public int PageSize { get; set; }
}