using AmyzFactory.App_Start;
using AmyzFactory.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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


        public ResultViewModel UploadArticleImage(HttpPostedFileBase imageFile, string apiPath)
        {
            using (var content = new MultipartFormDataContent())
            {
                MemoryStream target = new MemoryStream();
                imageFile.InputStream.CopyTo(target);
                byte[] Bytes = target.ToArray();


                imageFile.InputStream.Read(Bytes, 0, Bytes.Length);
                var fileContent = new ByteArrayContent(Bytes);
                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = imageFile.FileName };


                content.Add(fileContent);


                // content.Add(new StringContent(imageUniqueKey), "FileId");
                //content.Headers.Add("Key", "abc23sdflsdf");

                var response = GlobalVariables.WebApiClient.PostAsync(apiPath, content).Result;
                string result = response.Content.ReadAsStringAsync().Result;
                ResultViewModel resultVm = JsonConvert.DeserializeObject<ResultViewModel>(result);

                

                return resultVm;
            }
        }




        [HttpPost]
        public JsonResult CreateArticle(TextsViewModel model)
        {
            var imageUploadResult = this.UploadArticleImage(model.ImageFile, "Articles/UploadImage");

            if (imageUploadResult.IsSuccess == false)
            {
                model.Result = imageUploadResult;
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            ImagesViewModel imgVm =JsonConvert.DeserializeObject<ImagesViewModel> (imageUploadResult.Data.ToString());
            model.ImageUrl = imgVm.ImageUrl;
            model.ImageFile = null;

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