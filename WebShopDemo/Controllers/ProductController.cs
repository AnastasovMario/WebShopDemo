using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShopDemo.Core.Contracts;
using WebShopDemo.Core.Models;
using WebShopDemo.Core.Services;

namespace WebShopDemo.Controllers
{
	/// <summary>
	///	Web shop products
	/// </summary>
	public class ProductController : Controller
	{
		private readonly IProductService _productService;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="productService"></param>
		public ProductController(IProductService productService)
		{
			_productService = productService;
		}


		/// <summary>
		/// List all products 
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> Index()
		{
			var products = await _productService.GetAll();
			//подаваме view-то какво да включва и какво да изписва отгоре
			ViewData["Title"] = "Products";

			return View(products);
		}

		[HttpGet]
		//
		public IActionResult Add()
		{
			var model = new ProductDto();
            ViewData["Title"] = "Add new product";

            return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(ProductDto model)
		{
			//тази viewdata също трябва да се 
            ViewData["Title"] = "Add new product";

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			await _productService.Add(model);
			//зареждаме целия списък
			return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        //[Authorize(Policy = "CanDeleteProduct")]
        public async Task<IActionResult> Delete([FromForm] string id)
		{
            Guid idGuid = Guid.Parse(id);
            await _productService.Delete(idGuid);

            return RedirectToAction(nameof(Index));
        }
    }
}
