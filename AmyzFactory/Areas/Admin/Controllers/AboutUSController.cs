using AmyzFactory.App_Start;
using AmyzFactory.Models;
 
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;

namespace AmyzFactory.Areas.Admin.Controllers
{
    [AdminAuthorize(Roles = "Admins")]
    public class AboutUSController : Controller
    {
         
       
        // GET: Admin/AboutUS
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult AboutUsTexts()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetAboutUs").Result;
            TextsViewModel modelVm = response.Content.ReadAsAsync<TextsViewModel>().Result;
            return Json(modelVm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ResponsibiltyTexts()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetResponsibiltyTexts").Result;
            TextsViewModel modelVm = response.Content.ReadAsAsync<TextsViewModel>().Result;

            return Json(modelVm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadAboutUsImage(TextsViewModel image)
        {

        
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("AboutUS/UploadAboutUsImage", image).Result;


            TextsViewModel imageVm = response.Content.ReadAsAsync<TextsViewModel>().Result;


            return Json(imageVm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UploadResponsiobilityImage(TextsViewModel image)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("AboutUS/UploadResponsiobilityImage", image).Result;
            TextsViewModel imageVm = response.Content.ReadAsAsync<TextsViewModel>().Result;
            return Json(imageVm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAboutImage(string imageName)
        {
 
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Images/DeleteAboutUs?imageName="+imageName).Result;
            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);

        }



        public JsonResult DeleteResponsiobilityImage(string imageName)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Images/DeleteResponsiobilityImage?imageName="+ imageName).Result;
            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAboutImages()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Images/GetAboutUsImages").Result;
            List<ImagesViewModel> imagesVm = response.Content.ReadAsAsync<List<ImagesViewModel>>().Result;

            return Json(imagesVm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getResponsibilityImages()
        {

                  HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Images/GetResponsibilityImages").Result;
                List<TextsViewModel> imagesVm = response.Content.ReadAsAsync<List<TextsViewModel>>().Result;

            return Json(imagesVm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CreateUpdateAbout(TextsViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Information/CreateAboutUsTexts", model).Result;
            var result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateUpdateResponsipility(TextsViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Information/CreateResponsibilities", model).Result;
            var result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}