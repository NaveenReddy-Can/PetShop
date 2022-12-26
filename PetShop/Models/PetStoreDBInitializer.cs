using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PetShop.Models
{
    public class PetStoreDBInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            // Data Initializer created with a testUser as Admin
            ApplicationUser testUser = new ApplicationUser
            {
                UserName = "Naveen@gmail.com",
                Email = "Naveen@gmail.com",
                DateOfBirth = new DateTime(2000,1,1)
            };

            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

            IdentityRole role1 = new IdentityRole { Name = "Admin" };
            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStore);

            roleManager.Create(role1);
            userManager.Create(testUser, "test1234");
            userManager.AddToRole(testUser.Id, "Admin");

            // Pet with testuser as owner
            Pet pet1 = new Pet
            { 
                Name = "Peter",
                age = 6,
                IsMale = true,
                Breed = "Afgan Bull dog",
                Owner = testUser
            };
            context.Pets.Add(pet1);
            testUser.Pets.Add(pet1);

            // Pet without owner
            Pet pet2 = new Pet
            {
                Name = "Tommy",
                age = 7,
                IsMale = true,
                Breed = "American Bull dog"
            };
            Pet pet3 = new Pet
            {
                Name = "Jessy",
                age = 8,
                IsMale = false,
                Breed = "American Bull dog"
            };

            context.Pets.Add(pet2);
            context.Pets.Add(pet3);

            base.Seed(context);
        }
    }
}