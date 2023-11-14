using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using TraceyDawley.Helping_Classes;
using TraceyDawley.Models;

namespace TraceyDawley.Filters
{
    public class ValidationFilter : Attribute, IActionFilter
    {
        public int[] Roles;

        public ValidationFilter(int[] Roles = null)
        {
            this.Roles = Roles;
        }
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var services = filterContext.HttpContext.RequestServices;
            var gp = (GeneralPurpose)services.GetService(typeof(GeneralPurpose));

            User? LoggedinUser = gp.GetUserClaims();

            if (LoggedinUser != null)
            {
                if (!Roles.Contains((int)LoggedinUser.Role))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{
                            { "controller", "Auth" },{ "action", "Login" }, });
                }
            }
            else
            {
                var values = new RouteValueDictionary(new
                {
                    action = "Login",
                    controller = "Auth",
                    msg = "Session expired, Please login",
                    color = "red"
                });

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(values));
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }

}
