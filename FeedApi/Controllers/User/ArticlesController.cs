using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using AmyzFeed.FeedApi.Helpers;
using AutoMapper;
using FeedApi.Model;
using System.Collections.Generic;
using System.Net;
using System.Web;
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


        [HttpPost]
        public IHttpActionResult UploadImage()
        {
            ResultDomainModel result = new ResultDomainModel();

            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count < 1)
            {
                result.IsSuccess = false;
                result.Message = "حدث مشكلة فى رفع الصورة";
                return Content(HttpStatusCode.BadRequest, result);
            }


            result = this.imagesBusiness.uploadImage(httpRequest, Constans.ArticlesImageFolderPath, Constans.articleImageResponse,50);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }

        }


        public IHttpActionResult GetArticleById(int id)
        {
           ResultDomainModel result= this.addressBusiness.getArticleById(id, Constans.articleFilePath);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return Content(HttpStatusCode.BadRequest, result);
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
        [HttpPut]
        public IHttpActionResult UpdateArticle(TextsDomainModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest, new ResultDomainModel(false, "You pass empty article!"));
            }

            ResultDomainModel result = this.addressBusiness.UpdateArticle(model, Constans.articleFilePath);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return Content(HttpStatusCode.BadRequest, result);
        }


        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
          ResultDomainModel result=  this.addressBusiness.deleteContact(id, Constans.articleFilePath);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return Content(HttpStatusCode.BadRequest, result);
        }

    }
}
