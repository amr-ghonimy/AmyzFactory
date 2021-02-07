using AmyzFactory.App_Start;
using AmyzFactory.Models;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace AmyzFactory.Areas.Admin.Controllers
{
    [AdminAuthorize(Roles = "Admins")]

    public class QualitiesController : Controller
    {
       

        // GET: Admin/Qualities
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult QualityTexts()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Qualities/GetQualityTexts").Result;
            InformationViewModel modelVm = response.Content.ReadAsAsync<InformationViewModel>().Result;
 
            return Json(modelVm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteQualityImage(string imageName)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Categories/DeleteQualityImage",imageName).Result;
            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);

        }

        public JsonResult UploadQualityImage(InformationViewModel image)
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Categories/UploadQualityImage", image).Result;
            InformationViewModel imageVm = response.Content.ReadAsAsync<InformationViewModel>().Result;

            return Json(imageVm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getQualityImages()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Qualities/GetQualityImages").Result;
            List<InformationViewModel> imagesVm = response.Content.ReadAsAsync<List<InformationViewModel>>().Result;

            return Json(imagesVm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateUpdateQuality(InformationViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Qualities/CreateUpdateQuality", model).Result;
            model = response.Content.ReadAsAsync<InformationViewModel>().Result;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}