using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Repository.Data;
using AutoMapper;
using FeedApi.Helpers;
using FeedApi.Model;
using Newtonsoft.Json;
using System.Net;
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
                UserDomainModel userDm = (UserDomainModel) result.Data;

               userDm.Token = TokenManager.GenerateToken(userDm.UserName,userDm.Role);

                result.Data = userDm;

                return Ok(result);
            } else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }

        }
     


        [HttpPost]

        public IHttpActionResult Register(UserViemModel userModel)
        {
            if (ModelState.IsValid)
            {
                UserDomainModel dm = this.mapper.Map<UserDomainModel>(userModel);

                ResultDomainModel result = this.business.Register(dm);

                if (result.IsSuccess)
                {
                    UserDomainModel userDm = (UserDomainModel)result.Data;

                    userDm.Token = TokenManager.GenerateToken(userDm.UserName, userDm.Role);

                    result.Data = userDm;

                    return Ok(result);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }
            }

            return Content(HttpStatusCode.BadRequest, new ResultDomainModel { IsSuccess=false,Message="تأكد من ادخال البيانات بشكل صحبح!"});


        }




    }
}
