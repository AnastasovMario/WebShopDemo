using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebShopDemo.Models;

namespace WebShopDemo.Controllers
{
	public class HomeController : BaseController
    {
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}



		//public IActionResult Index()
		//{
		//	if (TempData.ContainsKey("LastAccessTime"))
		//	{
		//		TempData.Keep("LastAccessTime");

		//		//temp-data e абстракция върху кукитата
		//		return Ok(TempData.Peek("LastAccessTime"));
		//	}
		//	// записвам във въпросното куки datetime.now

		//	this.HttpContext.Response.Cookies.Append("myCookie", "Pesho", new CookieOptions()
		//	{

		//	});

		//	return View();
		//}
		[AllowAnonymous]
		public IActionResult Index()
		{
			//Това ще бъде записано в моята сесия
			//Можем да логнем user-a, и след като знаем кой е, какви неща да му показваме
			//Тук просто показваме как се сетва.
			this.HttpContext.Session.SetString("name", "pesho");

			return View();
		}

        [AllowAnonymous]
        public IActionResult Privacy()
		{
			string? name = this.HttpContext.Session.GetString("name");

			if (string.IsNullOrEmpty(name))
			{
				return Ok(name);
			}
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}