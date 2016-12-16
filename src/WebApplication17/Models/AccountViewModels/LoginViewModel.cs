using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication17.Models.AccountViewModels
{
    public class LoginViewModel
    {

        [Required]

        [Display(Name = "رقم الهوية")]
        [StringLength(14, ErrorMessage = "رقم الهوية يجب ان يكون من 10 الى 14 خانة")]
        public string NationalId { get; set; }

        [Required]

        [Display(Name = "رقم الجوال")]
        [StringLength(10, ErrorMessage = "رقم الجوال يجب ان يكون من 10 خانات و يبدأ ب 05")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
