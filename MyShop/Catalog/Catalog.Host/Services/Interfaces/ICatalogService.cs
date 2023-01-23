using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageSize, int pageIndex);
    Task<CatalogItemDto> GetCatalogItemByIdAsync(int id);
    Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemByBrandAsync(string brand, int pageSize, int pageIndex);
    Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemByTypeAsync(string type, int pageSize, int pageIndex);
    Task<PaginatedItemsResponse<CatalogBrandDto>> GetCatalogBrandsAsync(int pageSize, int pageIndex);
    Task<PaginatedItemsResponse<CatalogTypeDto>> GetCatalogTypesAsync(int pageSize, int pageIndex);
}