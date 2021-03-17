using AmyzFactory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace AmyzFactory.Controllers
{
    public class FeedProgramController : Controller
    {
        // GET: FeedProgram
        public ActionResult Index()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("FeedsProgram/GetAllFeeds").Result;

            List<FeedsProgramViewModel> feeds = response.Content.ReadAsAsync<List<FeedsProgramViewModel>>().Result;
 
            return View(feeds);
        }

        [HttpPost]
        public ActionResult Create()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("FeedsProgram/GetAllFeeds").Result;

            List<FeedsProgramViewModel> feeds = response.Content.ReadAsAsync<List<FeedsProgramViewModel>>().Result;

            return View(feeds);
        }

    }
}