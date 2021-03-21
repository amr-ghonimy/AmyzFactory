using AmyzFactory.App_Start;
using AmyzFactory.Models;
using AmyzFeed.Repository.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System.Net.Http;
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


        private void applyToken(UserViemModel user)
        {
            string token = user.Token;
            
            Session[SessionsModel.Token] = token;
            Session[SessionsModel.UserName] = user.UserName;
            Session[SessionsModel.UserId] = user.Id;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< ActionResult> Login(AdminViewModel model)
        {
            if (ModelState.IsValid)
            {


                HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Accounts/Login", model).Result;
                ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

                if (!result.IsSuccess)
                {
                    model.result = result;
                    return View(model);
                }
                else
                {

                   
                  //  JavaScriptSerializer js = new JavaScriptSerializer();
                    UserViemModel userVm = JsonConvert.DeserializeObject<UserViemModel>(result.Data.ToString());

                    if (userVm.Role != "Admins")
                    {
                        model.result = new ResultViewModel { IsSuccess = false, Message = "user name or password is invalid!!" };

                        return View(model);
                    }

                    ApplicationUser user = new ApplicationUser
                    {
                        Id = userVm.Id,
                        UserName = userVm.UserName,
                        Email = userVm.Email,
                    };

                    // store user is logined in our website
                                   await this.signIn(user);



                    this.applyToken(userVm);

                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                    {

                        return Redirect(model.ReturnUrl);

                    }
                    else
                    {
                        return RedirectToAction("Index", "HomePage");
                    }
                }


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