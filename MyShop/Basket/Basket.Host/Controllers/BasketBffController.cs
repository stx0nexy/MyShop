using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope("basket.basketbff")]
[Route(ComponentDefaults.DefaultRoute)]
public class BasketBffController : ControllerBase
{
    private readonly ILogger<BasketBffController> _logger;

    public BasketBffController(
        ILogger<BasketBffController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [AllowAnonymous]
    public Task TestLogAnonymousTask()
    {
        _logger.LogWarning("TestLogAnonymousTask");
        return Task.CompletedTask;
    }

    [HttpPost]
    public Task LogIdTask(int id)
    {
        _logger.LogWarning(id.ToString());
        return Task.CompletedTask;
    }
}