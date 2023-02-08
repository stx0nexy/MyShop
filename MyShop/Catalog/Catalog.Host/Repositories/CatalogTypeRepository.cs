using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CatalogTypeRepository : ICatalogTypeRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogTypeRepository> _logger;

    public CatalogTypeRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogTypeRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<int?> Add(string type)
    {
        var item = await _dbContext.AddAsync(new CatalogType()
        {
            Type = type
        });

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        CatalogType result = await _dbContext.CatalogTypes.FirstAsync(c => c.Id == id);
        _dbContext.CatalogTypes.Remove(result);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<CatalogType?> UpdateAsync(CatalogType catalogType)
    {
        _dbContext.Entry(catalogType).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return catalogType;
    }

    public async Task<CatalogType?> GetByIdAsync(int id)
    {
        var result = await _dbContext.CatalogTypes.Where(i => i.Id == id).FirstOrDefaultAsync();
        return result;
    }
}