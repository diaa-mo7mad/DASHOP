using DASHOP.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DASHOP.DAL.Utils
{
    public class UserSeedData : ISeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSeedData(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task DataSeed()
        {
            if (!await _userManager.Users.AnyAsync())
            {
                var user1 = new ApplicationUser
                {
                    UserName = "smsm1",
                    Email = "smsm@gmail.com",
                    FullName = "smsm",
                    EmailConfirmed = true,
                };
                var user2 = new ApplicationUser
                {
                    UserName = "ahmad",
                    Email = "ahmad@gmail.com",
                    FullName = "ahmadm",
                    EmailConfirmed = true,
                };
                var user3 = new ApplicationUser
                {
                    UserName = "yazan",
                    Email = "yazan@gmail.com",
                    FullName = "yazanb",
                    EmailConfirmed = true,
                };
                await _userManager.CreateAsync(user1, "P@ssw0rd123");
                await _userManager.CreateAsync(user2, "P@ssw0rd123");
                await _userManager.CreateAsync(user3, "P@ssw0rd123");

                await _userManager.AddToRoleAsync(user1, "SuperAdmin");
                await _userManager.AddToRoleAsync(user2, "Admin");
                await _userManager.AddToRoleAsync(user3, "User");

            }
        }
    }
}
