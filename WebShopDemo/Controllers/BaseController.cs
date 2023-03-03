using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebShopDemo.Core.Constants;

namespace WebShopDemo.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public string UserFirstName 
        {
            get 
            {
                string firstName = string.Empty;

                //Има смисъл да влизаме вътре, само ако има аутентикиран потребител.
                if ((User?.Identity?.IsAuthenticated ?? false && User.HasClaim(c => c.Type == ClaimTypeConstants.FirstName)))
                {
                    //Всички клеймове се записват на кукито по време на логин,
                    //ако се опитаме да вкараме клейм през базата, няма да можем, понеже не сме се логнали.
                    //те се пазят в session cookie-то, за да сменим клейма, трябва да сменим кукито.
                    firstName = User.Claims
                        .FirstOrDefault(c => c.Type == ClaimTypeConstants.FirstName)
                        ?.Value ?? firstName;
                }

                return firstName;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                ViewBag.UserFirstName = UserFirstName;
            }
            //Копираме това и го слагаме в login partial-а, за да може да се визуализира клейма, който сме записали
            ViewBag.UserFirstName = UserFirstName;

            base.OnActionExecuted(context);
        }
    }
}
