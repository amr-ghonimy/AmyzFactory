﻿using AmyzFactory.App_Start;
using AmyzFactory.Models;
using AmyzFeed.Repository.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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

         [UserAuthorize(Roles = "Users")]
        public ActionResult MyAccount()
        {
            return View();
        }

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

        public PartialViewResult GetCurrentUser()
        {
            string userID = User.Identity.GetUserId();
            ApplicationUser user = null;

            UserViemModel userVm = null;

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Accounts/GetUserByID?id=" + userID).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            if (result.Data != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                user = js.Deserialize<ApplicationUser>(result.Data.ToString());

                if (User.IsInRole("Users"))
                {
                    userVm = new UserViemModel
                    {
                        UserName = user.FirstName + ' ' + user.LastName
                    };
                }
                
            }
            return PartialView("~/Views/Shared/LoginNavBarSection.cshtml", userVm);
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserViemModel model)
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Accounts/Login", model).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            if (!result.IsSuccess)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }else
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                UserViemModel user = js.Deserialize<UserViemModel>(result.Data.ToString());

                // store user is logined in our website
            //    await this.signIn(user);

                UserViemModel userVm = new UserViemModel()
                {
                    Id = user.Id,
                    UserName = user.UserName
                };

                this.applyToken(userVm);

                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    result = new ResultViewModel
                    {
                        IsSuccess = true,
                        Message = model.ReturnUrl
                    };
 
                }else
                {
                    result = new ResultViewModel
                    {
                        IsSuccess = true,
                        Message = "../HomePreview/Index"
                    };

                }

                return Json(result, JsonRequestBehavior.AllowGet);


            }


        }



        private void  applyToken(UserViemModel user)
        {
            string token = user.Token;

            Session["TokenNumber"] = token;
            Session["UserName"] = user.UserName;
            Session["UserId"] = user.Id;
        }


        [HttpPost]
        public async Task<ActionResult> Register(UserViemModel userModel)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Accounts/Register", userModel).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;


            if (result.IsSuccess)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                ApplicationUser user = js.Deserialize<ApplicationUser>(result.Data.ToString());

                // store user is logined in our website
                await this.signIn(user);


                UserViemModel userVm = new UserViemModel()
                {
                    Id = user.Id,
                    UserName = user.UserName
                };

                this.applyToken(userVm);
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