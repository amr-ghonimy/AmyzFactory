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

        [HttpGet]
        public JsonResult GetArticles()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Articles/GetArticles").Result;
            var list = response.Content.ReadAsAsync<List<TextsViewModel>>().Result;

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateArticle(TextsViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Articles/CreateArticle", model).Result;
            var result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateArticle(TextsViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Articles/UpdateArticle", model).Result;
            var result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Articles/Delete?id="+id).Result;
            var result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}