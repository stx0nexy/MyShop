using AutoMapper;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogBrandService : BaseDataService<ApplicationDbContext>, ICatalogBrandService
{
    private readonly ICatalogBrandRepository _catalogBrandRepository;
    private readonly IMapper _mapper;

    public CatalogBrandService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogBrandRepository catalogBrandRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _catalogBrandRepository = catalogBrandRepository;
        _mapper = mapper;
    }

    public Task<int?> Add(string brand)
    {
        return ExecuteSafeAsync(() => _catalogBrandRepository.Add(brand));
    }

    public Task<bool> Delete(int id)
    {
        return ExecuteSafeAsync(() => _catalogBrandRepository.DeleteAsync(id));
    }

    public Task<CatalogBrandDto> Update(CatalogBrand catalogBrand)
    {
        return ExecuteSafeAsync(async () =>
        {
            var result = await _catalogBrandRepository.UpdateAsync(catalogBrand);
            return _mapper.Map<CatalogBrandDto>(result);
        });
    }
}