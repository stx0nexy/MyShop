namespace Catalog.Host.Models.Requests;

public class PaginatedItemsByTypeRequest
{
    public string Type { get; set; } = null!;

    public int PageIndex { get; set; }

    public int PageSize { get; set; }
}