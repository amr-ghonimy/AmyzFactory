using AmyzFactory.App_Start;
using AmyzFactory.Models;
using AmyzFeed.Repository.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AmyzFactory.Areas.Admin.Controllers
{
 
    public class AccountController : Controller
    {
        // GET: Admin/Account

       private ApplicationUserManager userManager;
        private RoleManager<IdentityRole> roleManager;

        public AccountController()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(dbContext);

            this.roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));


            this.userManager = new ApplicationUserManager(userStore);
        }


        public ActionResult Login(string returnUrl="")
        {
            return View(new AdminViewModel { ReturnUrl=returnUrl});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            var owinContext = Request.GetOwinContext();
            var authManager = owinContext.Authentication;
            authManager.SignOut("ApplicationCookie");
            Session.Abandon();

            return RedirectToAction("Login","Account");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< ActionResult> Login(AdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isAdminExists = await this.userManager.FindAsync(model.UserName, model.Password);

              
                if (isAdminExists != null)
                {
                    // this get role 
                    IList<string> rolesName = await userManager.GetRolesAsync(isAdminExists.Id);
                    string roleName = rolesName[0];

                    if (roleName != "Admins")
                    {
                        model.result = new ResultViewModel
                        {
                            IsSuccess = false,
                            Message = "Email or password is not correct!"
                        };
                        return View(model);

                    }

                    // store user is logined in our website
                    await this.signIn(isAdminExists);

                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                model.result = new ResultViewModel
                {
                    IsSuccess = false,
                    Message = "Email or password is not correct!"
                };

            }

            return View(model);
        }

        private async Task signIn(ApplicationUser appUser)
        {
            var identity= await this.userManager.CreateIdentityAsync(appUser, DefaultAuthenticationTypes.ApplicationCookie);
            var owinContext = Request.GetOwinContext();

            var authManager = owinContext.Authentication;
            authManager.SignIn(identity);
        }
    }
}