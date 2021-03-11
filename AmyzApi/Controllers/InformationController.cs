using AmyzFactory.Models;
using AmyzFeed.AmyzApi.Helpers;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Domain;
 using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace FeedApi.Controllers.User
{
    public class InformationController : ApiController
    {
        private IAddressesBusiness addressBusiness;
        private IImageBusiness imagesBusiness;

 
        public InformationController(IAddressesBusiness _addressBusiness, IImageBusiness _imagesBusiness)
        {
            this.addressBusiness = _addressBusiness;
            this.imagesBusiness = _imagesBusiness;
         }


        public TextsDomainModel GetFactoryInformation()
        {
            TextsDomainModel dm = this.addressBusiness.getTexts(Constans.infoFilePath);

            List<ImageDomainModel> images = this.imagesBusiness.getImages(Constans.infoImageFolderPath, Constans.infoImageResponse);

            if (images != null && images.Count > 0)
            {
                dm.ImageUrl = images[0].ImageUrl;
            }

             return dm;
        }

        public TextsDomainModel GetQualities()
        {
            TextsDomainModel dm = this.addressBusiness.getTexts(Constans.qualityFilePath);

            List<ImageDomainModel> images = this.imagesBusiness.getImages(Constans.qualityImageFolderPath, Constans.qualityImageResponse);

            if (images != null && images.Count > 0)
            {
                dm.ImageUrl = images[0].ImageUrl;
            }

            return dm;
        }

        public TextsDomainModel GetResponsibilityText()
        {
            TextsDomainModel dm = this.addressBusiness.getTexts(Constans.responsibiltyFilePath);

            List<ImageDomainModel> images = this.imagesBusiness.getImages(Constans.responsibiltyImageFolderPath, Constans.responsibiltyImageResponse);

            if (images != null && images.Count > 0)
            {
                dm.ImageUrl = images[0].ImageUrl;
            }

            return dm;
        }



        public IEnumerable<ContactDomainModel> GetEmails()
        {
            List<ContactDomainModel> dm = this.addressBusiness.getContacts(Constans.emailFilePath);

 
            return dm;
        }

        public IEnumerable<ContactDomainModel> GetPhones()
        {
            List<ContactDomainModel> dm = this.addressBusiness.getContacts(Constans.phonesFilePath);
            return dm;
        }


        [HttpPost]
        public IHttpActionResult CreateAboutUsTexts(TextsDomainModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, "No data came!"));
            }

            try
            {
                 ResultDomainModel result = this.addressBusiness.createOrUpdateFile(model, Constans.aboutFilePath);

                result.Data = model;

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, ex.Message));
            }
        }

        [HttpPost]
        public IHttpActionResult CreateQuality(TextsDomainModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, "No data came!"));
            }

            try
            {
                 ResultDomainModel result = this.addressBusiness.createOrUpdateFile(model, Constans.qualityFilePath);
                result.Data = model;


                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, ex.Message));
            }
        }

        [HttpPost]
        public IHttpActionResult CreateFactoryInfo(TextsDomainModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, "No data came!"));
            }

            try
            {
                 ResultDomainModel result = this.addressBusiness.createOrUpdateFile(model, Constans.infoFilePath);
                result.Data = model;

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, ex.Message));
            }
        }


        [HttpPost]
        public IHttpActionResult CreateUpdateResponsibility(TextsDomainModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, "No data came!"));
            }

            try
            {
                 ResultDomainModel result = this.addressBusiness.createOrUpdateFile(model, Constans.responsibiltyFilePath);
                result.Data = model;

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, ex.Message));
            }
        }

        [HttpPost]
        public IHttpActionResult CreatePhone(ContactDomainModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, "Phone is not have data!"));
            }

            try
            {
                 ResultDomainModel result = this.addressBusiness.createContact(model, Constans.phonesFilePath, 5);
                result.Data = model;

                return Ok(result);
            }
            catch (System.Exception ex)
            {

                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, ex.Message));
            }
        }

        [HttpPost]
        public IHttpActionResult CreateEmail(ContactDomainModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, "Email is not have data!"));
            }

            try
            {
                 ResultDomainModel result = this.addressBusiness.createContact(model, Constans.emailFilePath, 4);
                result.Data = model;

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, ex.Message));
            }
        }

        [HttpDelete]
        public IHttpActionResult DeleteEmail(int id)
        {
            try
            {
                ResultDomainModel dm = this.addressBusiness.deleteContact(id, Constans.emailFilePath);

                if (dm.IsSuccess)
                {
                    return Ok(dm);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, dm.Message);
                }

            }
            catch (System.Exception e)
            {

                return Content(HttpStatusCode.BadRequest, e.Message);
            }

        }

        [HttpDelete]
        public IHttpActionResult DeletePhone(int id)
        {
            try
            {
                ResultDomainModel dm = this.addressBusiness.deleteContact(id, Constans.phonesFilePath);

                if (dm.IsSuccess)
                {
                    return Ok(dm);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, dm.Message);
                }

            }
            catch (System.Exception e)
            {

                return Content(HttpStatusCode.BadRequest, e.Message);
            }

        }

    }
}
