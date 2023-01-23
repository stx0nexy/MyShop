using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogTypeRepository
{
    Task<int?> Add(string type);
    Task<bool> DeleteAsync(int id);
    Task<CatalogType> UpdateAsync(CatalogType catalogBrand);
}