using AmyzFactory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace AmyzFactory.Areas.Admin.Controllers
{
    public class ArticlesController : Controller
    {

      


        // GET: Admin/Articles
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult CreateArticle(TextsViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Articles/CreateArticle", model).Result;
            var result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}