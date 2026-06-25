using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Infrustructure.Seeder
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<User> _userManager)
        {
            var usersCount = await _userManager.Users.CountAsync();
            if (usersCount <= 0)
            {
                var defaultuser = new User()
                {
                    FullName = "schoolProject",
                    UserName = "admin",
                    Email = "admin@project.com",
                    Country = "Egypt",
                    PhoneNumber = "123456",
                    Address = "Mansora",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                await _userManager.CreateAsync(defaultuser, "Aa_123456");
                await _userManager.AddToRoleAsync(defaultuser, "Admin");
            }
        }
    }
}
