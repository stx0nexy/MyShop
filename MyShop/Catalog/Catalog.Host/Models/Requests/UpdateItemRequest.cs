using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;

namespace Catalog.Host.Models.Requests;

public class UpdateItemRequest
{
   public CatalogItem CatalogItem { get; set; } = null!;
}