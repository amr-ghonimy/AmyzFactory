using AmyzFactory.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;

namespace AmyzFactory.Controllers
{
    public class HomePreviewController : Controller
    {



        public PartialViewResult _GetSliderimages()
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Images/GetSliders").Result;
            List<ImagesViewModel> imagesVmList = response.Content.ReadAsAsync<List<ImagesViewModel>>().Result;


            return PartialView("~/Views/Shared/images/_sliders.cshtml", imagesVmList);
        }

        public ActionResult PeoductQuality()
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetQualities").Result;
            TextsViewModel qualityVm = response.Content.ReadAsAsync<TextsViewModel>().Result;

          
            return View(qualityVm);
        }

       
        public PartialViewResult _GetInfo()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetFactoryInformation").Result;
            TextsViewModel modelVm = response.Content.ReadAsAsync<TextsViewModel>().Result;

            return PartialView("~/Views/Shared/images/_info.cshtml", modelVm);
        }
        public PartialViewResult _GetDepartments()
        { 
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Departments/GetDepartments").Result;
            List<CategoryViewModel> categsVm = response.Content.ReadAsAsync<List<CategoryViewModel>>().Result;

            return PartialView("~/Views/Shared/_Categories.cshtml", categsVm);
        }

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult AboutUS()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetAboutUs").Result;
            TextsViewModel aboutUsVm = response.Content.ReadAsAsync<TextsViewModel>().Result;
           
            return View(aboutUsVm);
        }
        
        public ActionResult TechnocalSupport(int technicalID,string techName)
        {

            string url = "TechnicalSupport/GetTechnicalTextByTechID?id=" + technicalID ;

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync(url).Result;
            List<TechnicalTextViewModel> techsVm = response.Content.ReadAsAsync<List<TechnicalTextViewModel>>().Result;

            ViewBag.Name = techName;
             return View(techsVm);
        }

        private TextsViewModel getCensorshipHeader()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetCensorshipHeader").Result;
            TextsViewModel headerVm = response.Content.ReadAsAsync<TextsViewModel>().Result;

            return headerVm;
        }


        private TextsViewModel getCensorshipFooter()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetCensorshipFooter").Result;
            TextsViewModel footerVm = response.Content.ReadAsAsync<TextsViewModel>().Result;

            return footerVm;
        }

        public ActionResult Censorship()
        {
            TextsViewModel headerModel = this.getCensorshipHeader();
            TextsViewModel footerModel = this.getCensorshipFooter();
            ViewBag.FooterModel = footerModel;

            return View(headerModel);
        }






    }
}