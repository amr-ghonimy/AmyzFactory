using AmyzFactory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace AmyzFactory.Controllers
{
    public class QuantityController : Controller
    {
        // GET: Quantity
        public ActionResult Index()
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetQualities").Result;
            TextsViewModel qualityVm = response.Content.ReadAsAsync<TextsViewModel>().Result;
 
            return View(qualityVm);
        }
    }
}