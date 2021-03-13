using AmyzFactory.Models;
using AmyzFeed.AmyzApi.Helpers;
using AmyzFeed.Business.interfaces;
 using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Http;

namespace FeedApi.Controllers.User
{
    public class ImagesController : ApiController
    {
        private readonly IImageBusiness business;
 
        public ImagesController(IImageBusiness _productsBusiness)
        {
            this.business = _productsBusiness;
        }


        [HttpPost]
        public IHttpActionResult UploadSilderImage()
        {
            string folderPath = Constans.sliderImageFolderPath;
            return this.UploadImage(folderPath, Constans.sliderImageResponse, 10);
        }

        [HttpPost]
        public IHttpActionResult UploadInfoImage()
        {
            string folderPath = Constans.infoImageFolderPath;
            return this.UploadImage(folderPath, Constans.infoImageResponse, 1);
        }
        [HttpPost]
        public IHttpActionResult UploadQualityImage()
        {
            string folderPath = Constans.qualityImageFolderPath;
            return this.UploadImage(folderPath, Constans.qualityImageResponse, 1);
        }

        [HttpPost]
        public IHttpActionResult UploadResponsibiltyImage()
        {
            string folderPath = Constans.responsibiltyImageFolderPath;
            return this.UploadImage(folderPath, Constans.responsibiltyImageResponse, 1);
        }

        private IHttpActionResult UploadImage(string folderPath, string response, int imageCountValidation)
        {
            ResultDomainModel result = new ResultDomainModel();

            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count < 1)
            {
                result.IsSuccess = false;
                result.Message = "حدث مشكلة فى رفع الصورة";
                return Content(HttpStatusCode.BadRequest, result);
            }


            result = this.business.uploadImage(httpRequest, folderPath, response, imageCountValidation);

            if (result.IsSuccess)
            {
                ImageDomainModel imgDm = result.Data as ImageDomainModel;

                var request = HttpContext.Current.Request;
                var baseUrl = request.Url.Scheme + "://" + request.Url.Authority+"/";

 

                imgDm.ImageUrl = baseUrl + imgDm.ImageUrl;

                result.Data = imgDm;

                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }

        }

        // [CustomAuthFilter]
        public IEnumerable<ImageDomainModel> GetSliders()
        {
            List<ImageDomainModel> images = this.business.getImages(Constans.sliderImageFolderPath, Constans.sliderImageResponse);
 
            return images;
        }

        public IEnumerable<ImageDomainModel> GetInfoImages()
        {
            List<ImageDomainModel> images = this.business.getImages(Constans.infoImageFolderPath, Constans.infoImageResponse);

            return images;
        }
        public IEnumerable<ImageDomainModel> GetQualityImages()
        {
            List<ImageDomainModel> images = this.business.getImages(Constans.qualityImageFolderPath, Constans.qualityImageResponse);
 
            return images;
        }

        public IEnumerable<ImageDomainModel> GetResponsibilityImages()
        {
            List<ImageDomainModel> images = this.business.getImages(Constans.responsibiltyImageFolderPath, Constans.responsibiltyImageResponse);
 
            return images;
        }

        public IEnumerable<ImageDomainModel> GetAboutUsImages()
        {
            List<ImageDomainModel> images = this.business.getImages(Constans.aboutImageFolderPath, Constans.aboutImageResponse);
 
            return images;
        }
 


        [HttpDelete]
        public IHttpActionResult DeleteSlider(string imageName)
        {
            string imagePath = Constans.sliderImageFolderPath + imageName;

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

 

    }
}
