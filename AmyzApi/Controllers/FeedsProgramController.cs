using AmyzFactory.Models;
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


        [HttpPost]
        public IHttpActionResult  CreateFeed(FeedsProgramDomainModel model)
        {
            try
            {
                ResultDomainModel result = this.business.createFeedProgram(model);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel { IsSuccess=false,Message=e.Message});
            }

        }
    }
}
