using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
  using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FeedApi.Controllers.User
{
    public class ContactUsController : ApiController
    {
        private IQuestionaireBusiness business;
 
        public ContactUsController(IQuestionaireBusiness _business)
        {
            this.business = _business;
         }

        [HttpPost]
        public IHttpActionResult CreateQuestionaire(QuestionaireDomainModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "you pass empty value"));
            }

 
            ResultDomainModel resultDm = this.business.createQuestionaire(model);

            if (resultDm.IsSuccess)
            {
                return Ok(resultDm);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, resultDm);
            }
        }
    }
}
