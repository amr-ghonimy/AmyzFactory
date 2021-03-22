using AmyzApi.Helpers;
using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using System;
using System.Net;
using System.Web.Http;

namespace AmyzApi.Controllers
{
    public class AccountsController : ApiController
    {
        private IAuthBusiness business;

        public AccountsController(IAuthBusiness business)
        {
            this.business = business;
        }


        public IHttpActionResult GetUserByID(string id)
        {
            ResultDomainModel result = this.business.GetCurrentUser(id);

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
        public IHttpActionResult Login(UserDomainModel model)
        {
 

            ResultDomainModel result = this.business.login(model);

            if (result.IsSuccess)
            {
                UserDomainModel userDm = (UserDomainModel)result.Data;

                userDm.Token = TokenManager.GenerateToken(userDm.UserName, userDm.Role);
                userDm.ExpiresOn = DateTime.Now.AddDays(TokenManager.DaysOfActiveToken);

                result.Data = userDm;

                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }

        }



        [HttpPost]

        public IHttpActionResult Register(UserDomainModel userModel)
        {
            if (ModelState.IsValid)
            {
 
                ResultDomainModel result = this.business.Register(userModel);

                if (result.IsSuccess)
                {
                    UserDomainModel userDm = (UserDomainModel)result.Data;

                    userDm.Token = TokenManager.GenerateToken(userDm.UserName, userDm.Role);
                    userDm.ExpiresOn = DateTime.Now.AddDays(TokenManager.DaysOfActiveToken);

                    result.Data = userDm;

                    return Ok(result);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, result);
                }
            }

            return Content(HttpStatusCode.BadRequest, new ResultDomainModel { IsSuccess = false, Message = "تأكد من ادخال البيانات بشكل صحبح!" });


        }




    }
}
