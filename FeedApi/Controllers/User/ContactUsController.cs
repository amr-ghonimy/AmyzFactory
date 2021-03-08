using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using AutoMapper;
using FeedApi.Models;
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
        private readonly IMapper mapper;

        public ContactUsController(IQuestionaireBusiness _business)
        {
            this.business = _business;
            this.mapper = AutoMapperConfig.Mapper;
        }

        [HttpPost]
        public IHttpActionResult CreateQuestionaire(QuestionairViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false,"you pass empty value"));
            }

            QuestionaireDomainModel dm= this.mapper.Map<QuestionaireDomainModel>(model);

          ResultDomainModel resultDm=  this.business.createQuestionaire(dm);

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
