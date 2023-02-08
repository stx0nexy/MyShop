using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Repositories;

public class CatalogBrandRepository : ICatalogBrandRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogBrandRepository> _logger;

    public CatalogBrandRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogBrandRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<int?> Add(string brand)
    {
        var item = await _dbContext.AddAsync(new CatalogBrand()
        {
            Brand = brand
        });

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        CatalogBrand result = await _dbContext.CatalogBrands.FirstAsync(c => c.Id == id);
        _dbContext.CatalogBrands.Remove(result);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<CatalogBrand?> UpdateAsync(CatalogBrand catalogBrand)
    {
        _dbContext.Entry(catalogBrand).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return catalogBrand;
    }

    public async Task<CatalogBrand?> GetByIdAsync(int id)
    {
       var result = await _dbContext.CatalogBrands.Where(i => i.Id == id).FirstOrDefaultAsync();
       return result;
    }
}