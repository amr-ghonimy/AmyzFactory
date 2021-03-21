using AmyzFactory.App_Start;
using AmyzFactory.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Images/GetSliders").Result;
            var imagesVm = response.Content.ReadAsAsync<List<ImagesViewModel>>().Result;
      
            return Json(imagesVm, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetInfoImage()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Images/GetInfoImages").Result;
            var imagesVm = response.Content.ReadAsAsync<List<ImagesViewModel>>().Result;
            return Json(imagesVm, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetResponsibilityImage()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Images/GetResponsibilityImages").Result;
            var imagesVm = response.Content.ReadAsAsync<List<ImagesViewModel>>().Result;
            return Json(imagesVm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteSlider(string imageName)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Images/DeleteSlider?imageName="+ imageName).Result;

            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteInfoImage(string imageName)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Images/DeleteInfoImage?imageName=" + imageName).Result;

            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;


            return Json(resultVm, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteResponsiblityImage(string imageName)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Images/DeleteResponsiobilityImage?imageName=" + imageName).Result;

            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;


            return Json(resultVm, JsonRequestBehavior.AllowGet);
        }
        
        private TextsViewModel uploadImage(HttpPostedFileBase file, string apiPath)
        {

            using (var content = new MultipartFormDataContent())
            {
                MemoryStream target = new MemoryStream();
                file.InputStream.CopyTo(target);
                byte[] Bytes = target.ToArray();


                file.InputStream.Read(Bytes, 0, Bytes.Length);
                var fileContent = new ByteArrayContent(Bytes);
                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName };


                content.Add(fileContent);


               // content.Add(new StringContent(imageUniqueKey), "FileId");
                //content.Headers.Add("Key", "abc23sdflsdf");

                var response = GlobalVariables.WebApiClient.PostAsync(apiPath, content).Result;

                string result = response.Content.ReadAsStringAsync().Result;
                ResultViewModel resultVm = JsonConvert.DeserializeObject<ResultViewModel>(result);

                TextsViewModel textVm = null;

                if (resultVm.Data != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    ImagesViewModel imgVm = js.Deserialize<ImagesViewModel>(resultVm.Data.ToString());



                    textVm = new TextsViewModel()
                    {
                        Id = imgVm.Id,
                        ImageUrl = imgVm.ImageUrl,
                        Title = imgVm.Title,
                        Result = resultVm
                    };
                }
                else
                {

                    textVm = new TextsViewModel()
                    {
                        Result = resultVm
                    };
                }

                return textVm;
            }

        }

        [HttpPost]
        public JsonResult UploadSlider(TextsViewModel image)
        {
            TextsViewModel imageResult = this.uploadImage(image.ImageFile, "Images/UploadSilderImage/");

            return Json(imageResult, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult UploadInfoImage(TextsViewModel image)
        {
            TextsViewModel imageResult = this.uploadImage(image.ImageFile, "Images/UploadInfoImage/");

            return Json(imageResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadResponsiblityImage(TextsViewModel image)
        {
            TextsViewModel imageResult = this.uploadImage(image.ImageFile, "Images/UploadResponsibiltyImage/");

            return Json(imageResult, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CreatePhone(ContactViewModel phoneVm)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Information/CreatePhone", phoneVm).Result;

            var result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            phoneVm.Id = result.modelID;
            phoneVm.Result = result;

            return Json(phoneVm, JsonRequestBehavior.AllowGet);
          
        }


        [HttpGet]
        public ActionResult GetPhones()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetPhones").Result;

            var list = response.Content.ReadAsAsync<List<ContactViewModel>>().Result;


            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult CreateUpdateInfo(TextsViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Information/CreateFactoryInfo", model).Result;

            var result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            model.Id = result.modelID;
            model.Result = result;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateUpdateResponsibility(TextsViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Information/CreateUpdateResponsibility", model).Result;

            var result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            model.Id = result.modelID;
            model.Result = result;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateEmail(ContactViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Information/CreateEmail", model).Result;

            var result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            model.Id = result.modelID;
            model.Result = result;

            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetEmails()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetEmails").Result;

            var listVm = response.Content.ReadAsAsync<List<ContactViewModel>>().Result;

            return Json(listVm, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetSiteInfo()
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetFactoryInformation").Result;

            var model = response.Content.ReadAsAsync<TextsViewModel>().Result;

            return Json(model, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetResponsibilityText()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetResponsibilityText").Result;

            var model = response.Content.ReadAsAsync<TextsViewModel>().Result;

            return Json(model, JsonRequestBehavior.AllowGet);
        }







        [HttpGet]
        public JsonResult DeleteEmail(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Information/DeleteEmail?id="+id).Result;

            ResultViewModel resultVm  = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult DeletePhone(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Information/DeletePhone?id=" + id).Result;

            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);
         }
        

 
        

    }
}