using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Organic_Zone.Models;
using Owin;
using System.Configuration;

[assembly: OwinStartupAttribute(typeof(Organic_Zone.Startup))]
namespace Organic_Zone
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUser();
        }

        // Creating Admin Account
        private void CreateRolesAndUser()
        {
            ApplicationDbContext ADC = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ADC));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ADC));

            // Creating Admin Role and registring a default Admin User 
            if (!roleManager.RoleExists("Admin"))
            {

                // Creating Admin Role  
                IdentityRole role = new IdentityRole
                {
                    Name = "Admin"
                };
                roleManager.Create(role);

                // Creating Admin User (Super User) for the website
                ApplicationUser user = new ApplicationUser
                {
                    UserName = ConfigurationManager.AppSettings["AdminUsername"],
                    Email = ConfigurationManager.AppSettings["AdminEmail"]
                };

                // Admin Default Password
                string userPWD = ConfigurationManager.AppSettings["AdminPassword"];

                var chkUser = UserManager.Create(user, userPWD);

                // Assigning Admin Role to this user   
                if (chkUser.Succeeded)
                {
                    _ = UserManager.AddToRole(user.Id, "Admin");

                }
            }
        }
    }
}
