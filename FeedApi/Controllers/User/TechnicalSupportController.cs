using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Domain;
using AmyzFeed.FeedApi.Helpers;
using AutoMapper;
using FeedApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FeedApi.Controllers.User
{
    public class TechnicalSupportController : ApiController
    {
        private readonly IMapper mapper;
        private readonly ITechnicalBusiness techBusiness;


        public TechnicalSupportController(ITechnicalBusiness _techBusiness)
        {
            this.techBusiness = _techBusiness;
            this.mapper = AutoMapperConfig.Mapper;
        }


        public IEnumerable<TechnicalSupportViewModel> GetTechnicalSupports()
        {
            List<TechnicalSupportDomainModel> techsDm = this.techBusiness.getTechTextsList(Constans.technicalFilePath);

            List<TechnicalSupportViewModel> techsVm = this.mapper.Map<List<TechnicalSupportViewModel>>(techsDm);

            return techsVm;
        }

        public List<TechnicalTextViewModel> GetTechnicalTextByTechID(int id)
        {
           List<TechnicalTextDomainModel> techsDm= this.techBusiness.findTextxByTechID(id, Constans.technicalFilePath);

            return this.mapper.Map<List<TechnicalTextViewModel>>(techsDm);
        }
        

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                ResultDomainModel result = this.techBusiness.deleteTechTexts(id, Constans.technicalFilePath);

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

                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false,e.Message,id));
            }
        }


        [HttpPost]
        public IHttpActionResult UpdateTechTexts(TechnicalTextViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "You send data with null value"));
            }

            try
            {
                TechnicalTextDomainModel dm = this.mapper.Map<TechnicalTextDomainModel>(model);

                ResultDomainModel result = this.techBusiness.upsertTechTexts(dm, Constans.technicalFilePath);

                if (result.IsSuccess)
                {
                    result.Data = model;
                    return Ok(result);
                }else
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }

            }
            catch (Exception e )
            {

                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false,e.Message));
            }
        }

    }
}
