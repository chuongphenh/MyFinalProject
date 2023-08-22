using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using MyFinalProject.Models;

namespace MyFinalProject.Areas.Admin.Attributes
{
    public class CheckPermision : Attribute,IAuthorizationFilter
    {
        public string IdPer { get; set; }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var idUser = context.HttpContext.Session.GetInt32("admin_id_user");
            using (var db = new FinalDotnetProjectContext())
            {
                var record = db.UserPermisions.FirstOrDefault(item => item.IdUser == idUser && item.IdPer == IdPer);
                if (record == null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        { "area", "Admin" },
                        { "controller", "Users" },
                        { "action", "PermisionError" }
                    });
                }
            }
        }
    }
}
