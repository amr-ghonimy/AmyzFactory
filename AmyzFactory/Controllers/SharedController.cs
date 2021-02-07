using AmyzFactory.Models;

using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;

namespace AmyzFactory.Controllers
{
    public class SharedController : Controller
    {
 

        public PartialViewResult _SideBarAccount()
        {
            return PartialView("~/Views/Accounts/partial/_SideBarAccount.cshtml");
        }


        // GET: Shared
        public PartialViewResult _AllDepartmentsNavBar()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Shared/GetAllDepartmentsNavBar").Result;
            List<CategoryViewModel> departmentsVm = response.Content.ReadAsAsync<List<CategoryViewModel>>().Result;

            return PartialView("~/Views/Shared/nav_bar/_AllCategories.cshtml", departmentsVm);
        }


        public PartialViewResult _AllTechnicalsNavBar()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Shared/GetAllTechnicalsNavBar").Result;
            List<CategoryViewModel> technicalsVm = response.Content.ReadAsAsync<List<CategoryViewModel>>().Result;

            return PartialView("~/Views/Shared/nav_bar/_AllTechnicals.cshtml", technicalsVm);
        }


        public PartialViewResult _AllEmails()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Shared/GetAllEmails").Result;
            List<TextsViewModel> emailsVm = response.Content.ReadAsAsync<List<TextsViewModel>>().Result;


          
            return PartialView("~/Views/Shared/nav_bar/_AllEmails.cshtml", emailsVm);
        }

        public PartialViewResult _AllPhones()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Shared/GetAllPhones").Result;
            List<TextsViewModel> phonesVm = response.Content.ReadAsAsync<List<TextsViewModel>>().Result;

            return PartialView("~/Views/Shared/nav_bar/_AllPhones.cshtml", phonesVm);
        }

    }
}