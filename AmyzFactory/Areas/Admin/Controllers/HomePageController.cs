using AmyzFactory.App_Start;
using AmyzFactory.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace AmyzFactory.Areas.Admin.Controllers
{
    [AdminAuthorize(Roles = "Admins")]

    public class HomePageController : Controller
    {
      
      


        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetSliders()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Home/GetSliders").Result;
            var imagesVm = response.Content.ReadAsAsync<List<TextsViewModel>>().Result;
      
            return Json(imagesVm, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetInfo()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Home/GetInfo").Result;
            var imagesVm = response.Content.ReadAsAsync<List<TextsViewModel>>().Result;
            return Json(imagesVm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteSlider(string imageName)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Home/DeleteSlider", imageName).Result;

            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteInfo(string imageName)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Home/DeleteInfo", imageName).Result;

            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;


            return Json(resultVm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadSlider(TextsViewModel image)
        {

            var file = image.ImageFile;
            HttpResponseMessage response = null;

            using (var content = new MultipartFormDataContent())
            {
                MemoryStream target = new MemoryStream();
                file.InputStream.CopyTo(target);
                byte[] Bytes = target.ToArray();


                file.InputStream.Read(Bytes, 0, Bytes.Length);

                var fileContent = new ByteArrayContent(Bytes);
                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName };


                content.Add(fileContent);
                content.Add(new StringContent("123"), "FileId");


                response = GlobalVariables.WebApiClient.PostAsJsonAsync("Home/UploadSlider", content).Result;
            }

            TextsViewModel newImageVm = response.Content.ReadAsAsync<TextsViewModel>().Result;
            
            return Json(newImageVm, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult UploadInfoImage(TextsViewModel image)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Home/UploadInfoImage", image).Result;

            TextsViewModel newImageVm = response.Content.ReadAsAsync<TextsViewModel>().Result;

            return Json(newImageVm, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult CreatePhone(TextsViewModel phoneVm)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Home/CreatePhone", phoneVm).Result;

            phoneVm = response.Content.ReadAsAsync<TextsViewModel>().Result;

            return Json(phoneVm, JsonRequestBehavior.AllowGet);
          
        }


        [HttpGet]
        public ActionResult GetPhones()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Home/GetPhones").Result;

            var list = response.Content.ReadAsAsync<List<TextsViewModel>>().Result;


            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult CreateUpdateInfo(TextsViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Home/CreateUpdateInfo",model).Result;

            model = response.Content.ReadAsAsync<TextsViewModel>().Result;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        

        [HttpPost]
        public ActionResult CreateEmail(TextsViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Home/CreateEmail", model).Result;

            model = response.Content.ReadAsAsync<TextsViewModel>().Result;

            return Json(model, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult GetEmails()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Home/GetEmails").Result;

            var listVm = response.Content.ReadAsAsync<List<TextsViewModel>>().Result;

            return Json(listVm, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetSiteInfo()
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Home/GetSiteInfo").Result;

            var model = response.Content.ReadAsAsync<TextsViewModel>().Result;

            return Json(model, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public ActionResult CreateAccount(TextsViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Home/CreateAccount",model).Result;

            model = response.Content.ReadAsAsync<TextsViewModel>().Result;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAccounts()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Home/GetAccounts").Result;

            List<TextsViewModel> listVm = response.Content.ReadAsAsync<List<TextsViewModel>>().Result;

            return Json(listVm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteEmail(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Home/DeleteEmail",id).Result;

            ResultViewModel resultVm  = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult DeletePhone(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Home/DeletePhone", id).Result;

            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);
         }

        public JsonResult DeleteAccount(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Home/DeleteAccount", id).Result;

            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;
            
            return Json(resultVm, JsonRequestBehavior.AllowGet);
        }

 
        
    }
}