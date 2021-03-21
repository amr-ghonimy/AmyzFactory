using AmyzFactory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace AmyzFactory.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Admin/Base
 


        protected void ChangeHeader()
        {
            string tokenNumber = Session[SessionsModel.Token]?.ToString();
            GlobalVariables.WebApiClient.DefaultRequestHeaders.Clear();
            GlobalVariables.WebApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
               "Bearer", tokenNumber);
        }
    }
}