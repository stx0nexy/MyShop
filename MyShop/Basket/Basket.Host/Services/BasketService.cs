using Basket.Host.Models;
using Basket.Host.Services.Interfaces;
using Infrastructure.Services.Interfaces;
using MVC.Services.Interfaces;

namespace Basket.Host.Services;

public class BasketService : IBasketService
{
    private readonly IOptions<AppSettings> _settings;
    private readonly IInternalHttpClientService _httpClient;
    private readonly ILogger<BasketService> _logger;

    public BasketService(IInternalHttpClientService httpClient, ILogger<BasketService> logger, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;
    }

    public async Task<ItemResponse<int>?> AddItem(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var result = await _httpClient.SendAsync<ItemResponse<int>, ItemRequest>(
            $"{_settings.Value.CatalogUrl}/add",
            HttpMethod.Post,
            new ItemRequest()
            {
                Name = name,
                Description = description,
                Price = price,
                AvailableStock = availableStock,
                CatalogBrandId = catalogBrandId,
                PictureFileName = pictureFileName,
                CatalogTypeId = catalogTypeId,
            });
        return result;
    }
}