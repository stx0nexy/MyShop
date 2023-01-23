using Catalog.Host.Data.Entities;

namespace Catalog.Host.Models.Requests;

public class UpdateTypeRequest
{
    public CatalogType CatalogType { get; set; } = null!;
}