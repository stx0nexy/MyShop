namespace Basket.Host.Models;

public class ItemRequest
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public string PictureFileName { get; set; } = null!;

    public int CatalogTypeId { get; set; }

    public int CatalogBrandId { get; set; }

    public int AvailableStock { get; set; }
}