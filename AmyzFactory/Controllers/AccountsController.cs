using AmyzFactory.App_Start;
using AmyzFactory.Models;
using AmyzFeed.Repository.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AmyzFactory.Controllers
{
     public class AccountsController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        public AccountsController()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);

            this.userManager = new UserManager<ApplicationUser>(userStore);
        }

        // GET: /Account/Login

        public ActionResult Login(string returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        private async Task signIn(ApplicationUser appUser)
        {
            var identity = await this.userManager.CreateIdentityAsync(appUser, DefaultAuthenticationTypes.ApplicationCookie);
            var owinContext = Request.GetOwinContext();

            var authManager = owinContext.Authentication;
            authManager.SignIn(identity);
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserViemModel model)
        {
            

            var isUserExists = await this.userManager.FindAsync(model.UserName, model.Password);
            ResultViewModel result;
            if (isUserExists != null)
            {
                IList<string> rolesName = await userManager.GetRolesAsync(isUserExists.Id);
                string roleName = rolesName[0];

                if (roleName != "Users")
                {
                    result = new ResultViewModel
                    {
                        IsSuccess = false,
                        Message = "البريد الالكترونى أو كلمة السر غير صحيح"
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);

                }


                // store user is logined in our website
                await this.signIn(isUserExists);

                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    result = new ResultViewModel
                    {
                        IsSuccess = true,
                        Message = model.ReturnUrl
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                result = new ResultViewModel
                    {
                        IsSuccess = true,
                        Message = "/homePreview/Index"
                    };
            }else
            {
                result = new ResultViewModel
                {
                    IsSuccess = false,
                    Message = "البريد الالكترونى أو كلمة السر غير صحيح"
                };
            }
 
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UserViemModel userModel)
        {

            var name = userModel.FirstName.Trim() + "_" + userModel.LastName.Trim();


            var user = new ApplicationUser();

            user.Email = userModel.Email;
            user.UserName = name;
            user.PhoneNumber = userModel.PhoneNumber;
            user.Address = userModel.Address;
            user.PersonalID = userModel.PersonalId;
            user.Governorate = userModel.Governorate;
            user.IsActive = true;

            var check = userManager.Create(user, userModel.Password);

 


            ResultViewModel result;


            if (check.Succeeded)
            {
                var userID = user.Id;
                this.userManager.AddToRole(userID, "Users");
                
                result = new ResultViewModel()
                {
                    IsSuccess = true,
                    Message = "تم التسجيل بنجاح..."
                };


            }else
            {
                result = new ResultViewModel()
                {
                    IsSuccess = false,
                    Message = "لم يتم التسجيل كعضو جديد البريد الالكترونى او اسم المستخدم مستخدم من قبل"
                };
            }

           


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            var owinContext = Request.GetOwinContext();
            var authManager = owinContext.Authentication;
            authManager.SignOut("ApplicationCookie");
            Session.Abandon();

            return RedirectToAction("Index", "HomePreview");
        }

    }
}   