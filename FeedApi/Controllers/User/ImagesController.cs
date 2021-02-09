﻿using AmyzFactory.Models;
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

        public IEnumerable<ImagesViewModel> GetInfoImages()
        {
            List<ImageDomainModel> images = this.business.getImages(Constans.infoImageFolderPath, Constans.infoImageResponse);
            var result = this.mapper.Map<List<ImagesViewModel>>(images);

            return result;
        }
        public IEnumerable<ImagesViewModel> GetQualityImages()
        {
            List<ImageDomainModel> images = this.business.getImages(Constans.qualityImageFolderPath, Constans.qualityImageResponse);
            var result = this.mapper.Map<List<ImagesViewModel>>(images);

            return result;
        }

        public IEnumerable<ImagesViewModel> GetResponsibilityImages()
        {
            List<ImageDomainModel> images = this.business.getImages(Constans.responsibiltyImageFolderPath, Constans.responsibiltyImageResponse);
            var result = this.mapper.Map<List<ImagesViewModel>>(images);

            return result;
        }

        public IEnumerable<ImagesViewModel> GetAboutUsImages()
        {
            List<ImageDomainModel> images = this.business.getImages(Constans.aboutImageFolderPath, Constans.aboutImageResponse);
            var result = this.mapper.Map<List<ImagesViewModel>>(images);

            return result;
        }
        public IEnumerable<ImagesViewModel> GetCensorshipHeaderImages()
        {
            List<ImageDomainModel> images = this.business.getImages(Constans.censirshipHeaderImageFolderPath, Constans.censirshipHeaderImageResponse);
            var result = this.mapper.Map<List<ImagesViewModel>>(images);

            return result;
        }

        public IEnumerable<ImagesViewModel> GetCensorshiFooterImages()
        {
            List<ImageDomainModel> images = this.business.getImages(Constans.censirshipFooterImageFolderPath, Constans.censirshipFooterImageResponse);
            var result = this.mapper.Map<List<ImagesViewModel>>(images);

            return result;
        }



        [HttpDelete]
        public IHttpActionResult DeleteSlider(string imageName)
        {
            string imagePath = Constans.sliderImageFolderPath + imageName;

            ResultDomainModel result = this.business.deleteImage(imagePath);

            if (result.IsSuccess)
            {
                return Ok(result);
            }else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }

          
        }

        [HttpDelete]
        public IHttpActionResult DeleteInfoImage(string imageName)
        {
            string imagePath = Constans.infoImageFolderPath + imageName;

            ResultDomainModel result = this.business.deleteImage(imagePath);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }


        }
        [HttpDelete]
        public IHttpActionResult DeleteQualityImage(string imageName)
        {
            string imagePath = Constans.qualityImageFolderPath + imageName;

            ResultDomainModel result = this.business.deleteImage(imagePath);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }


        }

        [HttpDelete]
        public IHttpActionResult DeleteAboutUs(string imageName)
        {
            string imagePath = Constans.aboutImageFolderPath + imageName;

            ResultDomainModel result = this.business.deleteImage(imagePath);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }


        }


        [HttpDelete]
        public IHttpActionResult DeleteResponsiobilityImage(string imageName)
        {
            string imagePath = Constans.responsibiltyImageFolderPath + imageName;

            ResultDomainModel result = this.business.deleteImage(imagePath);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }


        }


        [HttpDelete]

        public IEnumerable<ImagesViewModel> DeleteCensorshipHeaderImages(string imageName)
        {
            List<ImageDomainModel> images = this.business.getImages(Constans.censirshipHeaderImageFolderPath, Constans.censirshipHeaderImageResponse);
            var result = this.mapper.Map<List<ImagesViewModel>>(images);

            return result;
        }
        [HttpDelete]

        public IEnumerable<ImagesViewModel> DeleteCensorshipFooterImages(string imageName)
        {
            List<ImageDomainModel> images = this.business.getImages(Constans.censirshipFooterImageFolderPath, Constans.censirshipFooterImageResponse);
            var result = this.mapper.Map<List<ImagesViewModel>>(images);

            return result;
        }

    }
}
