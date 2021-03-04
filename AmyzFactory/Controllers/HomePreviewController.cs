﻿using AmyzFactory.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;

namespace AmyzFactory.Controllers
{
    public class HomePreviewController : Controller
    {


        public JsonResult GetProductsCartSession()
        {
            var cardItemsList = Session["cartItems"] as List<ProductViewModel>;

            return Json(cardItemsList, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult _GetSliderimages()
        {

            string tokenNumber = Session["TokenNumber"]?.ToString() +":"+ Session["UserName"];

            GlobalVariables.WebApiClient.DefaultRequestHeaders.Clear();
            GlobalVariables.WebApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", tokenNumber);

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Images/GetSliders").Result;
              List<ImagesViewModel> imagesVmList = response.Content.ReadAsAsync<List<ImagesViewModel>>().Result;
        //    GlobalVariables.WebApiClient.DefaultRequestHeaders.Authorization = null;

            return PartialView("~/Views/Shared/images/_sliders.cshtml", imagesVmList);
        }

        public ActionResult ProductQuality()
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



        public ActionResult ArticleDetails(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Articles/GetArticleById?id="+id).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            TextsViewModel model = JsonConvert.DeserializeObject<TextsViewModel>(result.Data.ToString());

            return View(model);
        }
        public ActionResult Articles()
        { 
            return View(getAllArticles());
        }

        private List<TextsViewModel> getAllArticles()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Articles/GetArticles").Result;
            List<TextsViewModel> artickesVm = response.Content.ReadAsAsync<List<TextsViewModel>>().Result;
            return artickesVm;
        }

        public PartialViewResult _GetArticles()
        {
            return PartialView("~/Views/Shared/_Articles.cshtml", getAllArticles());
        }


        private List<ContactViewModel> getEmails()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetEmails").Result;
            List<ContactViewModel> emailsVm = response.Content.ReadAsAsync<List<ContactViewModel>>().Result;



            return emailsVm;
        }

        private List<ContactViewModel> getPhones()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetPhones").Result;
            List<ContactViewModel> phonesVm = response.Content.ReadAsAsync<List<ContactViewModel>>().Result;

            return phonesVm;
        }

        public ActionResult AboutUS()
        {
            ViewBag.Emails = this.getEmails();
            ViewBag.Phones = this.getPhones();

            return View();
        }
        
        public ActionResult TechnocalSupport(int technicalID,string techName)
        {

            string url = "TechnicalSupport/GetTechnicalTextByTechID?id=" + technicalID ;

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync(url).Result;
            List<TechnicalTextViewModel> techsVm = response.Content.ReadAsAsync<List<TechnicalTextViewModel>>().Result;

            ViewBag.Name = techName;
             return View(techsVm);
        }

     






    }
}