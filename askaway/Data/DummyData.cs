using askaway.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace askaway.Data
{
    public class DummyData
    {
        public static async Task Initialize(askawayDbContext context,
                              UserManager<ApplicationUser> userManager,
                              RoleManager<ApplicationRole> roleManager)
        {
            context.Database.EnsureCreated();

            String adminId1 = "";
            String adminId2 = "";

            string role1 = "Admin";
            string desc1 = "This is the administrator role";

            string role2 = "Member";
            string desc2 = "This is the members role";

            string password = "P@$$w0rd";

            if (await roleManager.FindByNameAsync(role1) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role1, desc1, DateTime.Now));
            }
            if (await roleManager.FindByNameAsync(role2) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(role2, desc2, DateTime.Now));
            }

            if (await userManager.FindByNameAsync("aa@aa.aa") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "aa@aa.aa",
                    Email = "aa@aa.aa",
                    RegistrationDate = DateTime.Now,
                    Birthday = DateTime.Now,
                    QuestionsAsked = 0,
                    StarsGiven = 0,
                    GivenAnswers = 0,
                    StarsReceived = 0,

                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role1);
                }
                adminId1 = user.Id;
            }

            if (await userManager.FindByNameAsync("bb@bb.bb") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "bb@bb.bb",
                    Email = "bb@bb.bb",
                    RegistrationDate = DateTime.Now,
                    Birthday = DateTime.Now,
                    QuestionsAsked = 0,
                    StarsGiven = 0,
                    GivenAnswers = 0,
                    StarsReceived = 0,
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role1);
                }
                adminId2 = user.Id;
            }

            if (await userManager.FindByNameAsync("mm@mm.mm") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "mm@mm.mm",
                    Email = "mm@mm.mm",
                    RegistrationDate = DateTime.Now,
                    Birthday = DateTime.Now,
                    QuestionsAsked = 0,
                    StarsGiven = 0,
                    GivenAnswers = 0,
                    StarsReceived = 0,
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
            }

            if (await userManager.FindByNameAsync("dd@dd.dd") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "dd@dd.dd",
                    Email = "dd@dd.dd",
                    RegistrationDate = DateTime.Now,
                    Birthday = DateTime.Now,
                    QuestionsAsked = 0,
                    StarsGiven = 0,
                    GivenAnswers = 0,
                    StarsReceived = 0,
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
            }

            string category1 = "Arts & Humanities";

            if(context.Categories.FirstOrDefault(c => c.CategoryName == category1) == null)
            {
                context.Categories.Add(new Category { CategoryName = category1});
                context.Categories.Add(new Category { CategoryName = "Beauty & Style" });
                context.Categories.Add(new Category { CategoryName = "Bussiness & Finance" });
                context.Categories.Add(new Category { CategoryName = "Cars & Transportation" });
                context.Categories.Add(new Category { CategoryName = "Computers & Internet" });
                context.Categories.Add(new Category { CategoryName = "Dining Out" });
                context.Categories.Add(new Category { CategoryName = "Education & Reference" });
                context.Categories.Add(new Category { CategoryName = "Entertainment & Music" });
                context.Categories.Add(new Category { CategoryName = "Environment" });
                context.Categories.Add(new Category { CategoryName = "Food & Drink" });
                context.Categories.Add(new Category { CategoryName = "Home & Garden" });
                context.Categories.Add(new Category { CategoryName = "News & Events" });
                context.Categories.Add(new Category { CategoryName = "Pets" });
                context.Categories.Add(new Category { CategoryName = "Politics & Government" });
                context.Categories.Add(new Category { CategoryName = "Science & Mathematics" });
                context.Categories.Add(new Category { CategoryName = "Society & Culture" });
                context.Categories.Add(new Category { CategoryName = "Sports" });
                context.Categories.Add(new Category { CategoryName = "Travel" });
                context.Categories.Add(new Category { CategoryName = "Others" });

                await context.SaveChangesAsync();
            }

        }
    }
}
    
