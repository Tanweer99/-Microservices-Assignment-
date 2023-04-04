using Common;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using ProductService.Service;
using System.Diagnostics;

namespace ProductService.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceProduct _productService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IServiceProduct productService, ILogger<HomeController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var productListResponse = await _productService.GetProductList();
            return View(productListResponse);
        }

        [Route("GetProduct/{productId}")]
        public async Task<ActionResult> Get(int productId)
        {
            var product = await _productService.GetProduct(productId);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}