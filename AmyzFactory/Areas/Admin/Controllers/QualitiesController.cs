using AmyzFactory.App_Start;
using AmyzFactory.Models;
using System.Collections.Generic;
using System.Net.Http;
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
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetQualities").Result;
            TextsViewModel modelVm = response.Content.ReadAsAsync<TextsViewModel>().Result;
 
            return Json(modelVm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteQualityImage(string imageName)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Images/DeleteQualityImage?imageName=" + imageName).Result;
            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);

        }

        public JsonResult UploadQualityImage(TextsViewModel image)
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Categories/UploadQualityImage", image).Result;
            TextsViewModel imageVm = response.Content.ReadAsAsync<TextsViewModel>().Result;

            return Json(imageVm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getQualityImages()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Images/GetQualityImages").Result;
            List<ImagesViewModel> imagesVm = response.Content.ReadAsAsync<List<ImagesViewModel>>().Result;

            return Json(imagesVm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateUpdateQuality(TextsViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Information/CreateQuality", model).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            model.Id = result.modelID;
            model.Result = result;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}