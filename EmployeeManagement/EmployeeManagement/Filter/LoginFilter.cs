using EmployeeManagement.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeManagement.Filter
{
    public class LoginFilter :IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {          
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("userName") == null)
            {
                if (context.Controller.GetType() != typeof(LoginController))
                {
                    context.Result = new RedirectResult("/Login/Login");
                }
            }

        }
    }
}
