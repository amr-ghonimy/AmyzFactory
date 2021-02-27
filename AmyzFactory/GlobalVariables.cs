using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace AmyzFactory
{
    public static class GlobalVariables
    {
        public static HttpClient WebApiClient = new HttpClient();
        public static string url = "http://localhost:57756/api/";

       static GlobalVariables()
        {
           
            WebApiClient.BaseAddress = new Uri(url) ;
            WebApiClient.DefaultRequestHeaders.Clear();
            WebApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            
        }
    }
}