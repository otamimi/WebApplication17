﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication17.Models;

namespace WebApplication17.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Refund>().ToTable("Refund");
            builder.Entity<Misfund>().ToTable("Misfund");
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Request> Requests { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Misfund> Misfunds { get; set; }
     
        public DbSet<Bank> Banks { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<RequiredDocument> RequiredDocuments { get; set; }
    }


}
