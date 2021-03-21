using AmyzFactory.App_Start;
using AmyzFactory.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AmyzFactory.Areas.Admin.Controllers
{
 [AdminAuthorize(Roles ="Admins")]
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
            TextsViewModel imageResult = this.uploadImage(image.ImageFile, "Images/UploadQualityImage/");

            return Json(imageResult, JsonRequestBehavior.AllowGet);
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


    }
}