using AmyzFactory.App_Start;
using AmyzFactory.Models;
using System.Net.Http;
using System.Web.Mvc;

namespace AmyzFactory.Areas.Admin.Controllers
{
    [AdminAuthorize(Roles = "Admins")]

    public class FeedsProgramController : Controller
    {
        // GET: Admin/FeedsProgram
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult CreateFeedProgram(FeedsProgramViewModel model)
        {

            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("FeedsProgram/CreateFeed",model).Result;
            ResultViewModel result = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}