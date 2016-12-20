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
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        public DbInitializer(ApplicationDbContext _context, RoleManager<IdentityRole> _roleManager)
        {
            context = _context;
            roleManager = _roleManager;
        }
        public async void Initialize(bool fresh, UserManager<ApplicationUser> userManager)
        {
            if (fresh)
            {
                // todo:delete all data from database
                context.Database.EnsureDeleted();
            }
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Users.Any())
                return; // DB has been seeded

            #region seed users and roles

          
            

            var roles = new[] {"Admin", "Applicant", "Student", "Role1", "Role2", "Role3", "Role4"};
            foreach (var role in roles)
            {
             
                if (!context.Roles.Any(r => r.Name == role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var usersnames = new[] {"Shahrani", "Riyadh", "Saleh", "student1", "applicant1"};
            var userStore = new UserStore<ApplicationUser>(context);
            
            foreach (var username in usersnames)
            {
                if (context.Users.Any(u => u.UserName == username)) continue;
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
               // var result = await userStore.CreateAsync(user);

            }
            context.SaveChanges();


            #endregion

            #region seed request required data

            //add request

            #endregion

            #region country

            if (!context.Countries.Any(country => country.Name.ToLower() == "KSA"))
                context.Countries.Add(new Country {Name = "KSA"});

            #endregion

            #region Required documents

           

            if (!context.RequiredDocuments.Any(doc => doc.Name == "PhotoID"))
                context.RequiredDocuments.Add(new RequiredDocument
                {
                    Name = "PhotoID",
                    Description = "seedData"
                });
            if (!context.RequiredDocuments.Any(doc => doc.Name == "Reciept"))
                context.RequiredDocuments.Add(new RequiredDocument
                {
                    Name = "Reciept",
                    Description = "seedData"
                });
            if (!context.RequiredDocuments.Any(doc => doc.Name == "DeathCertificate"))
                context.RequiredDocuments.Add(new RequiredDocument
                {
                    Name = "DeathCertificate",
                    Description = "seedData"
                });
            if (!context.RequiredDocuments.Any(doc => doc.Name == "procuration")) // wakala or tafweed
                context.RequiredDocuments.Add(new RequiredDocument
                {
                    Name = "procuration",
                    Description = "seedData"
                });
           
            #endregion

            #region bank and bankaccount

            var bank = new Bank {ArabicName = "SeedBank", Local = true};
            if (!context.Banks.Any())
            {
                context.Banks.Add(bank);
                
            }

            #endregion

            context.SaveChanges();

            

            
        }


    }
}