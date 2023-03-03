using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebShopDemo.Core.Constants;
using WebShopDemo.Core.Data.Models.Account;
using WebShopDemo.Models;

namespace WebShopDemo.Controllers
{
    public class AccountController : BaseController
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
			RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
        }

        [HttpGet]
        [AllowAnonymous]
        //За методите, които искаме да ползваме преди да се регистрираме, трябва да им сложим allow annonymous.
        public async Task<IActionResult> Register()
        {
            var model = new RegisterViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                EmailConfirmed = true,
                LastName = model.LastName,
                UserName = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);
            //по този начин добавяме клеймове - най-добрият вариант е да се добавя при регистрация
            await userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypeConstants.FirstName, user.FirstName ?? user.Email));

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }


            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        //get метод, който ни препраща във view-то
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            var model = new LoginViewModel()
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        //Post метод, който ни логва
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {                
                // по този начин  проверяваме паролата на user-a;
                var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
                    if (model.ReturnUrl != null)
                    {
                        //препращаме към страницата.
                        return Redirect(model.ReturnUrl);
                    }

                    //Връщаме към главанта страница
                    return RedirectToAction("Index", "Home");
                }
            }
            // не трябва да казваме какво се е случило.
            ModelState.AddModelError("", "Invalid login");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        //Създават се като напишем горе в полето /createRoles - 2:49:00 - обяснява как да се направят роли с interface
        public async Task<IActionResult> CreateRoles()
        {
            //създаваме ролите
            await roleManager.CreateAsync(new IdentityRole(RoleConstants.Manager));
            await roleManager.CreateAsync(new IdentityRole(RoleConstants.Supervisor));
            await roleManager.CreateAsync(new IdentityRole(RoleConstants.Administrator));


            return RedirectToAction("Index", "Home");
		}

        //това може да се направи и през базата, просто гледаме как се прави през самите managers.
        public async Task<IActionResult> AddUsersToRoles()
        {
            //това нещо трябва винаги да се случва през managers

            string email = "pesho@abv.bg";
            //string email1 = "mario@abv.bg";

			var user = await userManager.FindByEmailAsync(email);
			//var user1 = await userManager.FindByEmailAsync(email1);

			await userManager.AddToRoleAsync(user, RoleConstants.Manager);
			//Така се добавят повече от една роли на user
			//await userManager.AddToRolesAsync(user, new string[] { RoleConstants.Manager, RoleConstants.Supervisor});

			return RedirectToAction("Index", "Home");
		}
	}
}
