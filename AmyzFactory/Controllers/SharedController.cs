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
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Departments/GetDepartments").Result;
            List<CategoryViewModel> departmentsVm = response.Content.ReadAsAsync<List<CategoryViewModel>>().Result;

            return PartialView("~/Views/Shared/nav_bar/_AllCategories.cshtml", departmentsVm);
        }


        public PartialViewResult _AllTechnicalsNavBar()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Departments/GetTechnicals").Result;
            List<CategoryViewModel> technicalsVm = response.Content.ReadAsAsync<List<CategoryViewModel>>().Result;

            return PartialView("~/Views/Shared/nav_bar/_AllTechnicals.cshtml", technicalsVm);
        }


        public PartialViewResult _AllEmails()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetEmails").Result;
            List<ContactViewModel> emailsVm = response.Content.ReadAsAsync<List<ContactViewModel>>().Result;


          
            return PartialView("~/Views/Shared/nav_bar/_AllEmails.cshtml", emailsVm);
        }

        public PartialViewResult _AllPhones()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Information/GetPhones").Result;
            List<ContactViewModel> phonesVm = response.Content.ReadAsAsync<List<ContactViewModel>>().Result;

            return PartialView("~/Views/Shared/nav_bar/_AllPhones.cshtml", phonesVm);
        }

    }
}