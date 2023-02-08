using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogBrandRepository
{
    Task<int?> Add(string brand);
    Task<bool?> DeleteAsync(int id);
    Task<CatalogBrand?> UpdateAsync(CatalogBrand catalogBrand);
    Task<CatalogBrand?> GetByIdAsync(int id);
}