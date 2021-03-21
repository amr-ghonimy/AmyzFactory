using AmyzFactory.Models;
using AmyzFeed.Business.interfaces;
using AmyzFeed.Repository.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

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

            ApplicationUser isUserExists = this.userManager.Find(model.Email, model.Password);

            if (isUserExists != null)
            {
                IList<string> rolesName =  userManager.GetRoles(isUserExists.Id);
                string roleName = rolesName[0];

                UserDomainModel userDm = new UserDomainModel
                {
                    Id=isUserExists.Id,
                    FirstName=isUserExists.FirstName,
                    LastName=isUserExists.LastName,
                    UserName=isUserExists.UserName,
                    Password=isUserExists.PasswordHash,
                    Address=isUserExists.Address,
                    Email=isUserExists.Email,
                    PhoneNumber=isUserExists.PhoneNumber,
                    Role=roleName
                };


                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    result = new ResultDomainModel
                    {
                        IsSuccess = true,
                        Message = model.ReturnUrl,
                        Data = userDm
                    };
                }
                else
                {
                    result = new ResultDomainModel
                    {
                        IsSuccess = true,
                        Message = "/homePreview/Index",
                        Data = userDm
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





            var user = new ApplicationUser();
            user.FirstName = model.FirstName?.Trim();
            user.LastName = model.LastName?.Trim();
            user.Email = model.Email;
            user.UserName = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.IsActive = true;

            var isUserExists = userManager.FindByEmail(model.Email);
            ResultDomainModel result;

            if (isUserExists != null)
            {
                result = new ResultDomainModel()
                {
                    IsSuccess = false,
                    Message = "لم يتم التسجيل كعضو جديد البريد الالكترونى مستخدم من قبل",
                    Data = user
                };

            }
        

            var check = userManager.Create(user, model.Password);




           

            if (check.Succeeded)
            {
                var userID = user.Id;

                this.userManager.AddToRole(userID, "Users");

                UserDomainModel userDm = new UserDomainModel()
                {
                    Id = userID,
                    FirstName=user.FirstName,
                    LastName=user.LastName,
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

        public ResultDomainModel UpdateUserAddressAndPhone(string id, string address, string phone)
        {
            try
            {
               ApplicationUser user= this.userManager.FindById(id);
                user.Address = address;
                user.PhoneNumber = phone;

               userManager.Update(user);

                return new ResultDomainModel(isSuccess: true, message: "تم تحديث بيانات العميل..");
            }
            catch (Exception e)
            {
                return new ResultDomainModel(isSuccess: false, message: e.Message);

            }
        }
    }
}
