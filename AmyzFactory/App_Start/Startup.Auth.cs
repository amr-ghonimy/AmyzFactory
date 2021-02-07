using AmyzFactory.App_Start;
 using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace AmyzFactory
{
    public partial class Startup
    {

        public void ConfigureAuth(IAppBuilder app)
        {
 
            CookieAuthenticationOptions cookieAuthAdminOptions = new CookieAuthenticationOptions();
            cookieAuthAdminOptions.AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie;
            cookieAuthAdminOptions.LoginPath = new PathString("/Accounts/login");
            app.UseCookieAuthentication(cookieAuthAdminOptions);
 
        }
    }
}