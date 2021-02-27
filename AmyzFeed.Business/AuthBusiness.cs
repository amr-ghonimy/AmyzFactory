using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Repository;
using AmyzFeed.Repository.Data;
using AmyzFeed.Repository.Infrastructure.Contract;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmyzFeed.Business
{
   public class AuthBusiness:IAuthBusiness
    {
        
        private UserManager<ApplicationUser> userManager;
        public AuthBusiness()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);

            this.userManager = new UserManager<ApplicationUser>(userStore);
        }

        public ResultDomainModel GetCurrentUser(string Id)
        {          
            var user = this.userManager.FindById(Id); ;

            if (user == null)
            {
                return new ResultDomainModel(false, "لايوجد مستخدم حالى", Data: user);
            }else
            {
                return new ResultDomainModel(true, "تم ايجاد المستخدم", Data: user);
            }

        }

        public ResultDomainModel login(UserDomainModel model)
        {
            ResultDomainModel result;

            ApplicationUser isUserExists =  this.userManager.Find(model.UserName, model.Password);

            if (isUserExists != null)
            {
                IList<string> rolesName =  userManager.GetRoles(isUserExists.Id);
                string roleName = rolesName[0];

                if (roleName != "Users")
                {
                    result = new ResultDomainModel
                    {
                        IsSuccess = false,
                        Message = "البريد الالكترونى أو كلمة السر غير صحيح",
                        Data = isUserExists
                    };
                    return result;
                }

                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    result = new ResultDomainModel
                    {
                        IsSuccess = true,
                        Message = model.ReturnUrl,
                        Data = isUserExists
                    };
                }
                else
                {
                    result = new ResultDomainModel
                    {
                        IsSuccess = true,
                        Message = "/homePreview/Index",
                        Data = isUserExists
                    };
                }


 
            }
            else
            {
                result = new ResultDomainModel
                {
                    IsSuccess = false,
                    Message = "البريد الالكترونى أو كلمة السر غير صحيح",
                    Data = isUserExists

                };

            }

            return result;

        }

        public ResultDomainModel Register(UserDomainModel model)
        {
            var name = model.FirstName.Trim() + "_" + model.LastName.Trim();


            var user = new ApplicationUser();

            user.Email = model.Email;
            user.UserName = name;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.Governorate = model.Governorate;
            user.IsActive = true;

            var check = userManager.Create(user, model.Password);




            ResultDomainModel result;


            if (check.Succeeded)
            {
                var userID = user.Id;

                this.userManager.AddToRole(userID, "Users");

                UserDomainModel userDm = new UserDomainModel()
                {
                    Id = userID,
                    Address = user.Address,
                    UserName = user.UserName,
                    Password = user.PasswordHash,
                    Role= "Users",
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email
                };

                result = new ResultDomainModel()
                {
                    IsSuccess = true,
                    Message = "تم التسجيل بنجاح...",
                    Data = userDm
                };


 
            }
            else
            {
                result = new ResultDomainModel()
                {
                    IsSuccess = false,
                    Message = "لم يتم التسجيل كعضو جديد البريد الالكترونى او اسم المستخدم مستخدم من قبل",
                    Data = user
                };

             }

            return result;
        }


    }
}
