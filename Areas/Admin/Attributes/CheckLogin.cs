using Microsoft.AspNetCore.Mvc.Filters;//để kế thừa class ActionFilterAttribute
using MyFinalProject.Models;

namespace MyFinalProject.Areas.Admin.Attributes
{
    public class CheckLogin : ActionFilterAttribute //ke thua class ActionFilterAttribut
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //lấy giá trị của session email
            string name_user = context.HttpContext.Session.GetString("admin_userName");
            int? name_status = context.HttpContext.Session.GetInt32("admin_status");
            if (name_status == 0)
            {
                context.HttpContext.Response.Redirect("/Admin/Account/Login");
            }
            else
            {
                if (name_user == null)
                {
                    context.HttpContext.Response.Redirect("/Admin/Account/Login");
                }
            }

        }
    }
}
