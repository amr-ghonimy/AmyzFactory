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

    public class CensorshipController : Controller
    {

       
        // GET: Admin/Censorship
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult HeaderTexts()
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Censorship/GetHeaderTexts").Result;
            InformationViewModel modelVm = response.Content.ReadAsAsync<InformationViewModel>().Result;

            return Json(modelVm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FooterTexts()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Censorship/GetFooterTexts").Result;
            InformationViewModel modelVm = response.Content.ReadAsAsync<InformationViewModel>().Result;

            return Json(modelVm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getHeaderImages()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Censorship/GetHeaderImages").Result;
            List<InformationViewModel> imagesVm = response.Content.ReadAsAsync<List<InformationViewModel>>().Result;

            return Json(imagesVm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getFooterImages()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Censorship/GetFooterImages").Result;
            List<InformationViewModel> imagesVm = response.Content.ReadAsAsync<List<InformationViewModel>>().Result;

            return Json(imagesVm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CreateUpdateHeaderText(InformationViewModel model)
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Censorship/CreateUpdateHeaderText", model).Result;

            model = response.Content.ReadAsAsync<InformationViewModel>().Result;

            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CreateUpdateFooterText(InformationViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Censorship/CreateUpdateFooterText", model).Result;

            model = response.Content.ReadAsAsync<InformationViewModel>().Result;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteHeaderImage(string imageName)
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Censorship/DeleteHeaderImage", imageName).Result;

            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteFooterImage(string imageName)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Censorship/DeleteFooterImage", imageName).Result;

            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);

        }


        public JsonResult UploadCensorshipFooterImage(InformationViewModel image)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Censorship/UploadCensorshipFooterImage", image).Result;

            InformationViewModel imageVm  = response.Content.ReadAsAsync<InformationViewModel>().Result;

            return Json(imageVm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UploadCensorshipHeaderImage(InformationViewModel image)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Censorship/UploadCensorshipHeaderImage", image).Result;

            InformationViewModel imageVm = response.Content.ReadAsAsync<InformationViewModel>().Result;

            return Json(imageVm, JsonRequestBehavior.AllowGet);
        }
    }
}