using Catalog.Host.Models.Dtos;

namespace Catalog.Host.Models.Response;

public class ItemByIdResponse
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? PictureUrl { get; set; }

    public CatalogTypeDto? CatalogType { get; set; }

    public CatalogBrandDto? CatalogBrand { get; set; }

    public int AvailableStock { get; set; }
}