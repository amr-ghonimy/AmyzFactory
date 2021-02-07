using AmyzFeed.Repository.Data;
using FeedApi.App_Start;
using FeedApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FeedApi.Controllers
{
    public class AccountsController : ApiController
    {

        private ApplicationUserManager userManager;
        private RoleManager<IdentityRole> roleManager;

        public AccountsController()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(dbContext);

            this.roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));


            this.userManager = new ApplicationUserManager(userStore);
        }

        [HttpPost]
        public async Task<bool> LoginAdmin(AdminViewModel model)
        {
            ApplicationUser isAdminExists = await this.userManager.FindAsync(model.UserName, model.Password);

            if (isAdminExists != null)
            {
                // this get role 
                IList<string> rolesName = await userManager.GetRolesAsync(isAdminExists.Id);
                string roleName = rolesName[0];

                if (roleName != "Admins")
                {
                    return false;
                }

                return true;
            }

            return false;

        }

        private async Task signIn(ApplicationUser appUser)
        {
            var identity = await this.userManager.CreateIdentityAsync(appUser, DefaultAuthenticationTypes.ApplicationCookie);
            

        }

    }
}
