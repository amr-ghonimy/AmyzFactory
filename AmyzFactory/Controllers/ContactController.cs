using AmyzFactory.Models;
 
using System.Net.Http;
 
using System.Web.Mvc;

namespace AmyzFactory.Controllers
{
    public class ContactController : Controller
    {

      
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CreateQuest(QuestionairViewModel model)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("ContactUs/CreateQuestionaire", model).Result;

            ResultViewModel resultVm = response.Content.ReadAsAsync<ResultViewModel>().Result;

            return Json(resultVm, JsonRequestBehavior.AllowGet);
        }
    }
}