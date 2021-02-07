using AmyzFeed.Repository.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;


[assembly: OwinStartupAttribute(typeof(AmyzFactory.Startup))]
namespace AmyzFactory
{
    public partial class Startup
    {
      
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
               
        }

      
    }
}
