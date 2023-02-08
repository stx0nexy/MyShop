﻿using MVC.Dtos;
using MVC.Models.Enums;
using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Services;

public class CatalogService : ICatalogService
{
    private readonly IOptions<AppSettings> _settings;
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<CatalogService> _logger;

    public CatalogService(IHttpClientService httpClient, ILogger<CatalogService> logger, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;
    }

    public async Task<Catalog<CatalogItem>> GetCatalogItems(int page, int take, int? brand, int? type)
    {
        var filters = new Dictionary<CatalogTypeFilter, int>();

        if (brand.HasValue)
        {
            filters.Add(CatalogTypeFilter.Brand, brand.Value);
        }
        
        if (type.HasValue)
        {
            filters.Add(CatalogTypeFilter.Type, type.Value);
        }
        var result = await _httpClient.SendAsync<Catalog<CatalogItem>, PaginatedItemsRequest<CatalogTypeFilter>>($"{_settings.Value.CatalogUrl}/items",
           HttpMethod.Post, 
           new PaginatedItemsRequest<CatalogTypeFilter>()
            {
                PageIndex = page,
                PageSize = take,
                Filters = filters
            });

        return result;
    }

    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
        var result = await _httpClient.SendAsync<Filters<CatalogBrand>, PaginatedRequest>(
            $"{_settings.Value.CatalogUrl}/brands", HttpMethod.Post, null);
        IEnumerable<SelectListItem> selectList =

            from c in result.Data
            select new SelectListItem()
            {
                Selected = true,
                Text = c.Brand,
                Value = c.Id.ToString()
            };
        return selectList.ToArray();

    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
        var result = await _httpClient.SendAsync<Filters<CatalogType>, PaginatedRequest>(
            $"{_settings.Value.CatalogUrl}/types", HttpMethod.Post, null);
        IEnumerable<SelectListItem> selectList =

            from c in result.Data
            select new SelectListItem()
            {
                Selected = true,
                Text = c.Type,
                Value = c.Id.ToString()
            };
        return selectList.ToArray();
    }
}
