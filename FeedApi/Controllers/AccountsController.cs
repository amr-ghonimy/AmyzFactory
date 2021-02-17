using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Repository.Data;
using AutoMapper;
using FeedApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace FeedApi.Controllers
{
    public class AccountsController : ApiController
    {
        private IAuthBusiness business;
        private readonly IMapper mapper;

        public AccountsController(IAuthBusiness business)
        {
            this.business = business;
            this.mapper = AutoMapperConfig.Mapper;
        }


         public IHttpActionResult GetUserByID(string id)
        {
            ResultDomainModel result= this.business.GetCurrentUser(id);
 
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }

        }

        [HttpPost]
        public IHttpActionResult Login([FromBody]UserViemModel model)
        {
            UserDomainModel dm = this.mapper.Map<UserDomainModel>(model);


            ResultDomainModel result = this.business.login(dm);

            if (result.IsSuccess)
            {
                return Ok(result);
            } else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }

        }
     


        [HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult Register(UserViemModel userModel)
        {
            UserDomainModel dm = this.mapper.Map<UserDomainModel>(userModel);

            ResultDomainModel result = this.business.Register(dm);

            if (result.IsSuccess)
            {
                return Ok(result);
            }else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

     


    }
}
