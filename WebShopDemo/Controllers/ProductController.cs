using Microsoft.AspNetCore.Mvc;
using WebShopDemo.Core.Contracts;

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
	}
}
