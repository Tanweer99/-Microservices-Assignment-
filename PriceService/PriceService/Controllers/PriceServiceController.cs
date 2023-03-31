using Microsoft.AspNetCore.Mvc;
using PriceService.Models;
using PriceService.Service;

namespace PriceService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PriceController : ControllerBase
    {

        private readonly ILogger<PriceController> _logger;
        private readonly IPriceService _priceService;

        public PriceController(ILogger<PriceController> logger, IPriceService priceService)
        {
            _logger = logger;
            _priceService = priceService;
        }

        [HttpGet("GetPrice")]
        public IEnumerable<Price> Get()
        {
            return _priceService.Get();
        }
    }
}