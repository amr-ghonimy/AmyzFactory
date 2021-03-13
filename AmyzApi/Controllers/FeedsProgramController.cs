using AmyzFeed.Business.interfaces;
using AmyzFeed.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AmyzApi.Controllers
{
    public class FeedsProgramController : ApiController
    {
        private IFeedsProgramBusiness business;
        public FeedsProgramController(IFeedsProgramBusiness _business)
        {
            this.business = _business;
        }


        public List<FeedsProgramDomainModel> GetAllFeeds()
        {
          return  this.business.GetAllFeedsProgram();
        }
    }
}
