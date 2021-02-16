﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AmyzFactory.Models
{
 
    public class AdminDomainModel 
    {
        public int Id { get; set; }
         public string UserName { get; set; }
       

        public string Password { get; set; }

        public string ReturnUrl { get; set; }


        public ResultDomainModel result { get; set; }

    }

    public class UserDomainModel
    {
        public int Id { get; set; }

        
        public string UserName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }


        public string Email { get; set; }
        
        public string ReturnUrl { get; set; }


        public ResultDomainModel result { get; set; }

       
        public string PhoneNumber { get; set; }
 
        public string Address { get; set; }

       
        public string PersonalId { get; set; }
        
         
        public string Password { get; set; }
         
        public string ConfirmPassword { get; set; }

        public string Governorate { get; set; }

    }
}