using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;

[assembly: OwinStartup(typeof(AmyzFactory.App_Start.Startup))]

namespace AmyzFactory.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            CookieAuthenticationOptions cookieAuthAdminOptions = new CookieAuthenticationOptions();
            cookieAuthAdminOptions.AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie;
            cookieAuthAdminOptions.LoginPath = new PathString("/Accounts/login");
            app.UseCookieAuthentication(cookieAuthAdminOptions);
        }
    }
}
