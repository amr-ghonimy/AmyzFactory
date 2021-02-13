using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using AmyzFeed.FeedApi.Helpers;
using AutoMapper;
using FeedApi.Model;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace FeedApi.Controllers.User
{
    public class ArticlesController : ApiController
    {
        private IAddressesBusiness addressBusiness;
        private IImageBusiness imagesBusiness;

        private readonly IMapper mapper;

        public ArticlesController(IAddressesBusiness _addressBusiness, IImageBusiness _imagesBusiness)
        {
            this.addressBusiness = _addressBusiness;
            this.imagesBusiness = _imagesBusiness;

            this.mapper = AutoMapperConfig.Mapper;
        }


        public IEnumerable<TextsViewModel> GetArticles()
        {
            List<TextsDomainModel> list=this.addressBusiness.getArticles(Constans.articleFilePath);

            return this.mapper.Map<List<TextsViewModel>>(list);
        }
       

        [HttpPost]
        public IHttpActionResult CreateArticle(TextsViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest, "Yor Send null value");
            }

            try
            {
                TextsDomainModel dm = this.mapper.Map<TextsDomainModel>(model);

                ResultDomainModel result = this.addressBusiness.createArticle(dm, Constans.articleFilePath);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }
            }
            catch (System.Exception e)
            {

                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false,e.Message));
            }


        }

    }
}
