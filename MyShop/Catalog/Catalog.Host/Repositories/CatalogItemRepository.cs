using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize)
    {
        var totalItems = await _dbContext.CatalogItems
            .LongCountAsync();

        var itemsOnPage = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var item = await _dbContext.AddAsync(new CatalogItem
        {
            CatalogBrandId = catalogBrandId,
            CatalogTypeId = catalogTypeId,
            Description = description,
            Name = name,
            PictureFileName = pictureFileName,
            Price = price
        });

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<CatalogItem?> GetByIdAsync(int id)
    {
        var result = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Where(i => i.Id == id).FirstOrDefaultAsync();
        return result;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByBrandAsync(string brand, int pageIndex, int pageSize)
    {
        var totalItems = await _dbContext.CatalogItems
            .LongCountAsync();

        var result = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Where(w => w.CatalogBrand.Brand == brand)
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();
        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = result };
    }

    public async Task<PaginatedItems<CatalogItem>> GetByTypeAsync(string type, int pageIndex, int pageSize)
    {
        var totalItems = await _dbContext.CatalogItems
            .LongCountAsync();

        var result = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Where(w => w.CatalogType.Type == type)
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();
        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = result };
    }

    public async Task<PaginatedItems<CatalogBrand>> GetBrandsAsync(int pageIndex, int pageSize)
    {
        var totalItems = await _dbContext.CatalogBrands
            .LongCountAsync();

        var itemsOnPage = await _dbContext.CatalogBrands
            .OrderBy(c => c.Brand)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogBrand>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<PaginatedItems<CatalogType>> GetTypesAsync(int pageIndex, int pageSize)
    {
        var totalItems = await _dbContext.CatalogTypes
            .LongCountAsync();

        var itemsOnPage = await _dbContext.CatalogTypes
            .OrderBy(c => c.Type)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogType>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        CatalogItem result = await _dbContext.CatalogItems.FirstAsync(c => c.Id == id);
        _dbContext.CatalogItems.Remove(result);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<CatalogItem> UpdateAsync(CatalogItem catalogItem)
    {
        if (catalogItem.Id != default)
        {
            _dbContext.Entry(catalogItem).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        return catalogItem;
    }
}