using AmyzFactory.Models;
using AmyzFeed.Business;
using AmyzFeed.Business.interfaces;
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
    public class ImagesController : ApiController
    {
        private readonly IImageBusiness business;
        private readonly IMapper mapper;

        public ImagesController(IImageBusiness _productsBusiness)
        {
            this.business = _productsBusiness;
            this.mapper = AutoMapperConfig.Mapper;
        }


        public IEnumerable<ImagesViewModel> GetSliders()
        {
            List<ImageDomainModel> images = this.business.getImages(Constans.sliderImageFolderPath,Constans.sliderImageResponse);
            var result = this.mapper.Map<List<ImagesViewModel>>(images);

            return result;
        }

        


    }
}
