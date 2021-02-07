using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmyzFactory.App_Start
{
    public class AdminAuthorize: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // If you are authorized
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                // else redirect to your Area  specific login page
                filterContext.Result = new RedirectResult("~/Admin/Account/Login");
            }
        }
    }
}