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

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("HomePreview/GetSliders").Result;
            List<InformationViewModel> imagesVmList = response.Content.ReadAsAsync<List<InformationViewModel>>().Result;


            return PartialView("~/Views/Shared/images/_sliders.cshtml", imagesVmList);
        }

        public ActionResult PeoductQuality()
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("HomePreview/GetProductQuality").Result;
            InformationViewModel qualityVm = response.Content.ReadAsAsync<InformationViewModel>().Result;

          
            return View(qualityVm);
        }

       
        public PartialViewResult _GetInfoimage()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("HomePreview/GetInfoimage").Result;
            InformationViewModel modelVm = response.Content.ReadAsAsync<InformationViewModel>().Result;

            return PartialView("~/Views/Shared/images/_info.cshtml", modelVm);
        }
        public PartialViewResult _GetCategories()
        { 
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("HomePreview/GetCategories").Result;
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
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("HomePreview/GetAboutUS").Result;
            InformationViewModel aboutUsVm = response.Content.ReadAsAsync<InformationViewModel>().Result;
           
            return View(aboutUsVm);
        }
        
        public ActionResult TechnocalSupport(int technicalID,string techName)
        {

            string url = "HomePreview/GetTechnicalSupport?technicalID=" + technicalID + "&techName=" + techName;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync(url).Result;
            List<TechnicalTextViewModel> techsVm = response.Content.ReadAsAsync<List<TechnicalTextViewModel>>().Result;

            ViewBag.Name = techName;
             return View(techsVm);
        }

        private InformationViewModel getCensorshipHeader()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("HomePreview/GetCensorshipHeader").Result;
            InformationViewModel headerVm = response.Content.ReadAsAsync<InformationViewModel>().Result;

            return headerVm;
        }


        private InformationViewModel getCensorshipFooter()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("HomePreview/GetCensorshipFooter").Result;
            InformationViewModel footerVm = response.Content.ReadAsAsync<InformationViewModel>().Result;

            return footerVm;
        }

        public ActionResult Censorship()
        {
            InformationViewModel headerModel = this.getCensorshipHeader();
            InformationViewModel footerModel = this.getCensorshipFooter();
            ViewBag.FooterModel = footerModel;

            return View(headerModel);
        }






    }
}