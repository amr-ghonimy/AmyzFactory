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
    public class TechnicalsController : Controller
    {

 


     
        private SelectList getTecnicalsDropDown()
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Categories/GetTechnicals").Result;
             var techsVm = response.Content.ReadAsAsync<List<CategoryViewModel>>().Result;

           
            return new SelectList(techsVm, "Id", "Name");
        }


        // GET: Admin/Technicals
        public ActionResult Index()
        {
            ViewBag.TechsSelectList = this.getTecnicalsDropDown();
            return View();
        }


        [HttpGet]
        public JsonResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Technicals/Delete", id).Result;
            ResultViewModel resultVm= response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateTechTexts(TechnicalTextViewModel text)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Technicals/UpdateTechTexts", text).Result;
            text = response.Content.ReadAsAsync<TechnicalTextViewModel>().Result;


            return Json(text, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetTechnicals()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Technicals/GetTechnicalsTexts").Result;
            List<TechnicalSupportViewModel> techsVm = response.Content.ReadAsAsync<List<TechnicalSupportViewModel>>().Result;

            return Json(techsVm, JsonRequestBehavior.AllowGet);
        }


    }
}