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

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetCensorshipHeaderTexts").Result;
            TextsViewModel modelVm = response.Content.ReadAsAsync<TextsViewModel>().Result;

            return Json(modelVm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FooterTexts()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetCensorshipFooterTexts").Result;
            TextsViewModel modelVm = response.Content.ReadAsAsync<TextsViewModel>().Result;

            return Json(modelVm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getHeaderImages()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Images/GetCensorshipHeaderImages").Result;
            List<ImagesViewModel> imagesVm = response.Content.ReadAsAsync<List<ImagesViewModel>>().Result;

            return Json(imagesVm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getFooterImages()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Images/GetCensorshiFooterImages").Result;
            List<ImagesViewModel> imagesVm = response.Content.ReadAsAsync<List<ImagesViewModel>>().Result;

            return Json(imagesVm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CreateUpdateHeaderText(TextsViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Information/CreateUpdateCensorshipHeaderText", model).Result;

            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            model.Result = result;
            model.Id = result.modelID;
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CreateUpdateFooterText(TextsViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Information/CreateUpdateCensorshipFooterText", model).Result;

            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            model.Result = result;
            model.Id = result.modelID;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteHeaderImage(string imageName)
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Censorship/DeleteCensorshipHeaderImages?imageName="+ imageName).Result;

            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteFooterImage(string imageName)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Censorship/DeleteCensorshipFooterImages?imageName=" + imageName).Result;

            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);

        }


        public JsonResult UploadCensorshipFooterImage(TextsViewModel image)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Censorship/UploadCensorshipFooterImage", image).Result;

            TextsViewModel imageVm  = response.Content.ReadAsAsync<TextsViewModel>().Result;

            return Json(imageVm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UploadCensorshipHeaderImage(TextsViewModel image)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Censorship/UploadCensorshipHeaderImage", image).Result;

            TextsViewModel imageVm = response.Content.ReadAsAsync<TextsViewModel>().Result;

            return Json(imageVm, JsonRequestBehavior.AllowGet);
        }
    }
}