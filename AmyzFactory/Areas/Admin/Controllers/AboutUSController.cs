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
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("AboutUS/GetAboutUsTexts").Result;
            InformationViewModel modelVm = response.Content.ReadAsAsync<InformationViewModel>().Result;
            return Json(modelVm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ResponsibiltyTexts()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("AboutUS/GetResponsibiltyTexts").Result;
            InformationViewModel modelVm = response.Content.ReadAsAsync<InformationViewModel>().Result;

            return Json(modelVm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadAboutUsImage(InformationViewModel image)
        {

        
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("AboutUS/UploadAboutUsImage", image).Result;


            InformationViewModel imageVm = response.Content.ReadAsAsync<InformationViewModel>().Result;


            return Json(imageVm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UploadResponsiobilityImage(InformationViewModel image)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("AboutUS/UploadResponsiobilityImage", image).Result;
            InformationViewModel imageVm = response.Content.ReadAsAsync<InformationViewModel>().Result;
            return Json(imageVm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAboutImage(string imageName)
        {
 
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("AboutUS/DeleteAboutImage",imageName).Result;
            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);

        }



        public JsonResult DeleteResponsiobilityImage(string imageName)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("AboutUS/DeleteResponsiobilityImage", imageName).Result;
            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getAboutImages()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("AboutUS/getAboutImages").Result;
            List<InformationViewModel> imagesVm = response.Content.ReadAsAsync<List<InformationViewModel>>().Result;

            return Json(imagesVm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getResponsibilityImages()
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("AboutUS/getResponsibilityImages").Result;
            List<InformationViewModel> imagesVm = response.Content.ReadAsAsync<List<InformationViewModel>>().Result;

            return Json(imagesVm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CreateUpdateAbout(InformationViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("AboutUS/CreateUpdateAbout", model).Result;
            model = response.Content.ReadAsAsync<InformationViewModel>().Result;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateUpdateResponsipility(InformationViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("AboutUS/CreateUpdateResponsipility", model).Result;
            model = response.Content.ReadAsAsync<InformationViewModel>().Result;

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}