using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApplication17.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "رقم الهوية/الاقامة")]
        public string NationalId { get; set; }
        [Display(Name = "اسم مقدم الطلب")]
        public string FullName { get; set; }

    }
   
}
