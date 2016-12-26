using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WebApplication17.Models;

namespace WebApplication17.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext applicationDbContextcontext;
        private readonly RoleManager<IdentityRole> roleManager;
        public DbInitializer( RoleManager<IdentityRole> roleManager,   ApplicationDbContext applicationDbContextcontext)
        {
         
            this.roleManager = roleManager;
            this.applicationDbContextcontext = applicationDbContextcontext;
        }
        public async void Initialize(bool fresh, UserManager<ApplicationUser> userManager)
        {
            if (fresh)
            {
                // todo:delete all data from database
                applicationDbContextcontext.Database.EnsureDeleted();
            }
            applicationDbContextcontext.Database.EnsureCreated();

            // Look for any students.
            if (applicationDbContextcontext.Users.Any())
                return; // DB has been seeded

            #region seed users and roles

          
            

            var roles = new[] {"Admin", "Applicant", "Student", "Role1", "Role2", "Role3", "Role4"};
            foreach (var role in roles)
            {
             
                if (!applicationDbContextcontext.Roles.Any(r => r.Name == role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var usersnames = new[] {"Shahrani", "Riyadh", "Saleh", "student1", "applicant1"};
            var userStore = new UserStore<ApplicationUser>(applicationDbContextcontext);
            
            foreach (var username in usersnames)
            {
                if (applicationDbContextcontext.Users.Any(u => u.UserName == username)) continue;
                var idnumber = Guid.NewGuid().ToString();
                var user = new ApplicationUser
                {
                    UserName = username.ToLower(),
                    EmailConfirmed = false,
                    FullName = "NoNameAvaiable.SeedData",
                    NationalId = idnumber.Substring(0, 12),
                    PhoneNumber = "3213216546"
                };
                
                var res =await  userManager.CreateAsync(user, "123123");
                if(username== "applicant1")
                await userManager.AddToRoleAsync(user, "Applicant");
                // var result = await userStore.CreateAsync(user);

            }
            applicationDbContextcontext.SaveChanges();


            #endregion

            #region seed request required data

            //add request

            #endregion

            #region country

            if (!applicationDbContextcontext.Countries.Any(country => country.Name.ToLower() == "KSA"))
                applicationDbContextcontext.Countries.Add(new Country {Name = "KSA"});

            #endregion

            #region Required documents

           

            if (!applicationDbContextcontext.RequiredDocuments.Any(doc => doc.Name == "PhotoID"))
                applicationDbContextcontext.RequiredDocuments.Add(new RequiredDocument
                {
                    Name = "PhotoID",
                    Description = "seedData"
                });
            if (!applicationDbContextcontext.RequiredDocuments.Any(doc => doc.Name == "Reciept"))
                applicationDbContextcontext.RequiredDocuments.Add(new RequiredDocument
                {
                    Name = "Reciept",
                    Description = "seedData"
                });
            if (!applicationDbContextcontext.RequiredDocuments.Any(doc => doc.Name == "DeathCertificate"))
                applicationDbContextcontext.RequiredDocuments.Add(new RequiredDocument
                {
                    Name = "DeathCertificate",
                    Description = "seedData"
                });
            if (!applicationDbContextcontext.RequiredDocuments.Any(doc => doc.Name == "procuration")) // wakala or tafweed
                applicationDbContextcontext.RequiredDocuments.Add(new RequiredDocument
                {
                    Name = "procuration",
                    Description = "seedData"
                });
           
            #endregion

            #region bank and bankaccount

            var bank = new Bank {ArabicName = "SeedBank", Type = BankType.Local};
            if (!applicationDbContextcontext.Banks.Any())
            {
                applicationDbContextcontext.Banks.Add(bank);
                
            }

            #endregion

            applicationDbContextcontext.SaveChanges();

            

            
        }


    }
}