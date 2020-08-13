using EmployeeLeave.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeLeave
{
    public static class SeedData
    {
        public static void Seed(UserManager<Employee> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUser(userManager);

        }
        public static void SeedUser(UserManager<Employee> userManager)
        {
            if (userManager.FindByNameAsync("admin@localhost.com").Result == null)
            {
                var user = new Employee
                {
                    UserName = "admin@localhost.com",
                    Email = "admin@localhost.com"
                };
                var result=userManager.CreateAsync(user,"P@ssword1").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                var result=roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Employee").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Employee"
                };
               var result=roleManager.CreateAsync(role).Result;
            }
        }

    }
}
