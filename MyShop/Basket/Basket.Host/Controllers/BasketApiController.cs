using Basket.Host.Models;
using Basket.Host.Services.Interfaces;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope("basket.basketapi")]
[Route(ComponentDefaults.DefaultRoute)]
public class BasketApiController : ControllerBase
{
    private readonly ILogger<BasketApiController> _logger;
    private readonly IBasketService _basketService;

    public BasketApiController(
        ILogger<BasketApiController> logger,
        IBasketService basketService)
    {
        _logger = logger;
        _basketService = basketService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ItemResponse<int>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddItem(ItemRequest item)
    {
        var result = await _basketService.AddItem(item.Name, item.Description, item.Price, item.AvailableStock, item.CatalogBrandId, item.CatalogTypeId, item.PictureFileName);
        return Ok(result);
    }
}