
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModelClasses;
using RepositoryClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class DbInitializer : IDbInitializer
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;

        public DbInitializer(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();

                }
            }
            catch (Exception) { }
            if (!_roleManager.RoleExistsAsync(WebSiteRoles.Website_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.Website_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.Website_Patient)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.Website_Doctor)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "NavinSah",
                    Email = "nabinsah703@gmail.com"
                }, "nabinsah703@").GetAwaiter().GetResult();

                var AppUser = _context.applicationUsers.FirstOrDefault(f => f.Email == "nabinsah703@gmail.com");
                if (AppUser != null)
                {

                    _userManager.AddToRoleAsync(AppUser, WebSiteRoles.Website_Admin);
                }
            }
        }
    }
}
