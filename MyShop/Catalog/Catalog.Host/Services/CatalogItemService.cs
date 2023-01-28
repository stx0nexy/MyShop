using AutoMapper;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogItemService : BaseDataService<ApplicationDbContext>, ICatalogItemService
{
    private readonly ICatalogItemRepository _catalogItemRepository;
    private readonly ICatalogBrandRepository _catalogBrandRepository;
    private readonly ICatalogTypeRepository _catalogTypeRepository;
    private readonly IMapper _mapper;

    public CatalogItemService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository,
        ICatalogBrandRepository catalogBrandRepository,
        ICatalogTypeRepository catalogTypeRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
        _catalogBrandRepository = catalogBrandRepository;
        _catalogTypeRepository = catalogTypeRepository;
        _mapper = mapper;
    }

    public Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        return ExecuteSafeAsync(() => _catalogItemRepository.Add(name, description, price, availableStock, catalogBrandId, catalogTypeId, pictureFileName));
    }

    public Task<bool> Delete(int id)
    {
        return ExecuteSafeAsync(() => _catalogItemRepository.DeleteAsync(id));
    }

    public Task<CatalogItemDto> Update(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        return ExecuteSafeAsync(async () =>
        {
            var catalogBrand = await _catalogBrandRepository.GetByIdAsync(catalogBrandId);
            var catalogType = await _catalogTypeRepository.GetByIdAsync(catalogTypeId);
            var result = await _catalogItemRepository.UpdateAsync(new CatalogItem()
            {
                Id = id,
                Name = name,
                Description = description,
                Price = price,
                AvailableStock = availableStock,
                CatalogBrand = catalogBrand,
                CatalogType = catalogType,
                PictureFileName = pictureFileName
            });
            return _mapper.Map<CatalogItemDto>(result);
        });
    }
}