using Microsoft.AspNetCore.Mvc;
using WebShopDemo.Core.Contracts;
using WebShopDemo.Core.Models;

namespace WebShopDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService _productService)
        {
            productService = _productService;
        }
        
        /// <summary>
        /// Get All products
        /// </summary>
        /// <returns></returns>
        /// В реално приложение е много важно това.
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200, StatusCode = StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductDto>))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await productService.GetAll());
        }
    }
}
