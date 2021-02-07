using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using System.Security.Claims;
using System.Threading.Tasks;


namespace AmyzFeed.Repository.Data
{
    public class ApplicationUser : IdentityUser
    {

        // here i will add some colomn to user table
        public bool IsActive { get; set; }
        public string PersonalID { get; set; }
        public string Address{ get; set; }
        public string Governorate { get; set; }
         
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("Amyz_FeedIdentities", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}