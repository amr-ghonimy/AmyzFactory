using AmyzFeed.FeedApi.Helpers;
using AmyzFeed.Repository.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Web;
using System.Web.Http;

namespace FeedApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public void CreateUseres()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser();
            user.Email = "amyz_feed@yahoo.com";
            user.UserName = "amyz";

            var check = userManager.Create(user, "123456");

            if (check.Succeeded)
            {
                userManager.AddToRole(user.Id, "Admins");

            }
        }

        public void CreateRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            IdentityRole role;

            if (!roleManager.RoleExists("Admin"))
            {
                role = new IdentityRole();
                role.Name = "Admins";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Users"))
            {
                role = new IdentityRole();
                role.Name = "Users";
                roleManager.Create(role);
            }

        }





        private void setPathes()
        {


            Constans.emailFilePath = Server.MapPath(@"Files\emails.txt");
            Constans.phonesFilePath = Server.MapPath(@"Files\phones.txt");
            Constans.accountsFilePath = Server.MapPath(@"Files\accounts.txt");
            Constans.infoFilePath = Server.MapPath(@"Files\info.txt");
            Constans.aboutFilePath = Server.MapPath(@"Files\about.txt");
            Constans.qualityFilePath = Server.MapPath(@"Files\quality.txt");
            Constans.aboutFilePath = Server.MapPath(@"Files\about.txt");
            Constans.technicalFilePath = Server.MapPath(@"Files\technicals.txt");
            Constans.responsibiltyFilePath = Server.MapPath(@"Files\responsibilty.txt");
            Constans.censirshipHeaderFilePath = Server.MapPath(@"Files\cenHeader.txt");
            Constans.censirshipFooterFilePath = Server.MapPath(@"Files\cenFooter.txt");
            Constans.articleFilePath = Server.MapPath(@"Files\articles.txt");

            Constans.sliderImageFolderPath = Server.MapPath(Constans.sliderImageResponse);
            Constans.qualityImageFolderPath = Server.MapPath(Constans.qualityImageResponse);
            Constans.aboutImageFolderPath = Server.MapPath(Constans.aboutImageResponse);
            Constans.infoImageFolderPath = Server.MapPath(Constans.infoImageResponse);
            Constans.responsibiltyImageFolderPath = Server.MapPath(Constans.responsibiltyImageResponse);
            Constans.censirshipFooterImageFolderPath = Server.MapPath(Constans.censirshipFooterImageResponse);
            Constans.censirshipHeaderImageFolderPath = Server.MapPath(Constans.censirshipHeaderImageResponse);

        }


        protected void Application_Start()
        {
 
            this.CreateRoles();
            this.CreateUseres();
            this.setPathes();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperConfig.init();

        }
    }
}
