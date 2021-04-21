using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using vroom.AppDbContext;
using vroom.Helpers;
using vroom.Models;

namespace vroom.Data
{
    public class DBInitializer : IDBInitializer
    {
        private readonly VroomDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DBInitializer(VroomDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async void Initialize()
        {
            //add pending migration if exists
            if (_db.Database.GetPendingMigrations().Count() > 0)
            { _db.Database.Migrate(); }

            //Exits if role already exists
            if (_db.Roles.Any(r => r.Name == Helpers.Roles.Admin)) return;

            //create Role admin
            _roleManager.CreateAsync(new IdentityRole(Roles.Admin)).GetAwaiter().GetResult();

            //create admin user
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = Roles.Admin,
                Email = "Admin@Gmail.com",
                EmailConfirmed = true,
            }, "Admin@123").GetAwaiter().GetResult();

            //Assign role to admin user
            await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync("Admin"), Roles.Admin);
        }
    }
}
