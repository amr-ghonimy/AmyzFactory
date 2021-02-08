using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Domain;
using AmyzFeed.FeedApi.Helpers;
using AutoMapper;
using FeedApi.Model;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace FeedApi.Controllers.User
{
    public class InformationController : ApiController
    {
        private IAddressesBusiness addressBusiness;
        private IImageBusiness imagesBusiness;

        private readonly IMapper mapper;
 
        public InformationController(IAddressesBusiness _addressBusiness, IImageBusiness _imagesBusiness)
        {
            this.addressBusiness = _addressBusiness;
            this.imagesBusiness = _imagesBusiness;

            this.mapper = AutoMapperConfig.Mapper;
        }


        public TextsViewModel GetFactoryInformation()
        {
           TextsDomainModel dm= this.addressBusiness.getTexts(Constans.infoFilePath);

           List< ImageDomainModel > images= this.imagesBusiness.getImages(Constans.infoImageFolderPath, Constans.infoImageResponse);

            if (images != null && images.Count > 0)
            {
                dm.ImageUrl = images[0].ImageUrl;
            }

            TextsViewModel vm = this.mapper.Map<TextsViewModel>(dm);
            return vm;
        }
        public TextsViewModel GetAboutUs()
        {
            TextsDomainModel dm = this.addressBusiness.getTexts(Constans.aboutFilePath);

            List<ImageDomainModel> images = this.imagesBusiness.getImages(Constans.aboutImageFolderPath, Constans.aboutImageResponse);

            if (images != null && images.Count > 0)
            {
                dm.ImageUrl = images[0].ImageUrl;
            }

            TextsViewModel vm = this.mapper.Map<TextsViewModel>(dm);
            return vm;
        }

        public TextsViewModel GetQualities()
        {
            TextsDomainModel dm = this.addressBusiness.getTexts(Constans.qualityFilePath);

            List<ImageDomainModel> images = this.imagesBusiness.getImages(Constans.qualityImageFolderPath, Constans.qualityImageResponse);

            if (images != null && images.Count > 0)
            {
                dm.ImageUrl = images[0].ImageUrl;
            }

            TextsViewModel vm = this.mapper.Map<TextsViewModel>(dm);
            return vm;
        }
        public TextsViewModel GetResponsibiltyTexts()
        {
            TextsDomainModel dm = this.addressBusiness.getTexts(Constans.responsibiltyFilePath);

            List<ImageDomainModel> images = this.imagesBusiness.getImages(Constans.responsibiltyImageFolderPath, Constans.responsibiltyImageResponse);

            if (images != null && images.Count > 0)
            {
                dm.ImageUrl = images[0].ImageUrl;
            }

            TextsViewModel vm = this.mapper.Map<TextsViewModel>(dm);
            return vm;
        }

        public TextsViewModel GetCensorshipHeaderTexts()
        {
            TextsDomainModel dm = this.addressBusiness.getTexts(Constans.censirshipHeaderFilePath);

            List<ImageDomainModel> images = this.imagesBusiness.getImages(Constans.censirshipHeaderImageFolderPath, Constans.censirshipHeaderImageResponse);

            if (images != null && images.Count > 0)
            {
                dm.ImageUrl = images[0].ImageUrl;
            }

            TextsViewModel vm = this.mapper.Map<TextsViewModel>(dm);
            return vm;
        }

        public TextsViewModel GetCensorshipFooterTexts()
        {
            TextsDomainModel dm = this.addressBusiness.getTexts(Constans.censirshipFooterFilePath);

            List<ImageDomainModel> images = this.imagesBusiness.getImages(Constans.censirshipFooterImageFolderPath, Constans.censirshipFooterImageResponse);

            if (images != null && images.Count > 0)
            {
                dm.ImageUrl = images[0].ImageUrl;
            }

            TextsViewModel vm = this.mapper.Map<TextsViewModel>(dm);
            return vm;
        }



        public IEnumerable<ContactViewModel> GetAccounts()
        {
            List<ContactDomainModel> dm = this.addressBusiness.getContacts(Constans.accountsFilePath);

            List<ContactViewModel> vm = this.mapper.Map<List<ContactViewModel>>(dm);

            return vm;
        }

        public IEnumerable<ContactViewModel> GetEmails()
        {
            List<ContactDomainModel> dm = this.addressBusiness.getContacts(Constans.emailFilePath);

            List<ContactViewModel> vm = this.mapper.Map<List<ContactViewModel>>(dm);

            return vm;
        }
        public IEnumerable<ContactViewModel> GetPhones()
        {
            List<ContactDomainModel> dm = this.addressBusiness.getContacts(Constans.phonesFilePath);

            List<ContactViewModel> vm = this.mapper.Map<List<ContactViewModel>>(dm);

            return vm;
        }


        [HttpPost]
        public IHttpActionResult CreateAboutUsTexts(TextsViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, "No data came!"));
            }

            try
            {
                TextsDomainModel dm = this.mapper.Map<TextsDomainModel>(model);
                ResultDomainModel result = this.addressBusiness.createOrUpdateFile(dm,Constans.aboutFilePath);

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
        public IHttpActionResult CreateResponsibilities(TextsViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, "No data came!"));
            }

            try
            {
                TextsDomainModel dm = this.mapper.Map<TextsDomainModel>(model);
                ResultDomainModel result = this.addressBusiness.createOrUpdateFile(dm, Constans.responsibiltyFilePath);

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
        public IHttpActionResult CreateQuality(TextsViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, "No data came!"));
            }

            try
            {
                TextsDomainModel dm = this.mapper.Map<TextsDomainModel>(model);
                ResultDomainModel result = this.addressBusiness.createOrUpdateFile(dm, Constans.qualityFilePath);
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
        public IHttpActionResult CreateFactoryInfo(TextsViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, "No data came!"));
            }

            try
            {
                TextsDomainModel dm = this.mapper.Map<TextsDomainModel>(model);
                ResultDomainModel result = this.addressBusiness.createOrUpdateFile(dm, Constans.infoFilePath);
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
        public IHttpActionResult CreateUpdateCensorshipHeaderText(TextsViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, "No data came!"));
            }

            try
            {
                TextsDomainModel dm = this.mapper.Map<TextsDomainModel>(model);
                ResultDomainModel result = this.addressBusiness.createOrUpdateFile(dm, Constans.censirshipHeaderFilePath);

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
        public IHttpActionResult CreateUpdateCensorshipFooterText(TextsViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, "No data came!"));
            }

            try
            {
                TextsDomainModel dm = this.mapper.Map<TextsDomainModel>(model);
                ResultDomainModel result = this.addressBusiness.createOrUpdateFile(dm, Constans.censirshipFooterFilePath);

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
        public IHttpActionResult CreateAccount(ContactViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, "Account is not have data!"));
            }

            try
            {
                ContactDomainModel dm = this.mapper.Map<ContactDomainModel>(model);
                ResultDomainModel result = this.addressBusiness.createContact(dm, Constans.accountsFilePath, 5);
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
        public IHttpActionResult CreatePhone(ContactViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, "Phone is not have data!"));
            }

            try
            {
                ContactDomainModel dm = this.mapper.Map<ContactDomainModel>(model);
                ResultDomainModel result = this.addressBusiness.createContact(dm, Constans.phonesFilePath, 5);
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
        public IHttpActionResult CreateEmail(ContactViewModel model)
        {
            if (model == null)
            {
                return Content(HttpStatusCode.BadRequest,
                    new ResultDomainModel(false, "Email is not have data!"));
            }

            try
            {
                ContactDomainModel dm = this.mapper.Map<ContactDomainModel>(model);
                ResultDomainModel result = this.addressBusiness.createContact(dm, Constans.emailFilePath, 4);
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
                }else
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

        [HttpDelete]
        public IHttpActionResult DeleteAccount(int id)
        {
            try
            {
                ResultDomainModel dm = this.addressBusiness.deleteContact(id, Constans.accountsFilePath);

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
