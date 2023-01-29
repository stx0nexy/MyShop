using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogTypeService
{
    Task<int?> Add(string type);
    Task<bool?> Delete(int id);
    Task<CatalogTypeDto> Update(int id, string type);
}