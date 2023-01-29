using AutoMapper;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogTypeService : BaseDataService<ApplicationDbContext>, ICatalogTypeService
{
    private readonly ICatalogTypeRepository _catalogTypeRepository;
    private readonly IMapper _mapper;

    public CatalogTypeService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogTypeRepository catalogTypeRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _catalogTypeRepository = catalogTypeRepository;
        _mapper = mapper;
    }

    public Task<int?> Add(string brand)
    {
        return ExecuteSafeAsync(() => _catalogTypeRepository.Add(brand));
    }

    public Task<bool?> Delete(int id)
    {
        return ExecuteSafeAsync(() => _catalogTypeRepository.DeleteAsync(id));
    }

    public Task<CatalogTypeDto> Update(int id, string type)
    {
        return ExecuteSafeAsync(async () =>
        {
            var result = await _catalogTypeRepository.UpdateAsync(new CatalogType() { Id = id, Type = type });
            return _mapper.Map<CatalogTypeDto>(result);
        });
    }
}