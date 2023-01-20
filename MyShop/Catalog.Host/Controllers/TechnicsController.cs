using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TechnicsController : ControllerBase
    {
        private static readonly string[] Types = new[]
        {
            "air conditioner", "dishwasher", "laundry dryer", "drying cabinet",
            "freezer", "refrigerator", "cooker", "water heater", "washing machine", "microwave"
        };

        private static readonly string[] Brands = new[]
        {
            "AEG", "ASKO", "Kuppersbusch", "Maunfeld", "Kaiser", "Franke", "Graude", "BEKO", "Bosch", "Haier"
        };

        private readonly ILogger<TechnicsController> _logger;

        public TechnicsController(ILogger<TechnicsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Technics> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Technics()
                {
                    Type = Types[Random.Shared.Next(Types.Length)],
                    Brand = Brands[Random.Shared.Next(Brands.Length)],
                    Price = Random.Shared.Next(100, 1000),
                    Warranty = Random.Shared.Next(0, 10)
                })
                .ToArray();
        }
    }
}